using System.Collections;
using UnityEngine;

/// <summary>
/// Tracks an entity in story coordinates, periodically moves it toward (0,0),
/// and shows EnemyDot on the radar only while inside the radar story bounds.
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("References")]
    public Camera radarCamera;
    public GameObject enemyDot;

    [Header("Audio")]
    public AudioSource inRadarRangeAudio;
    public AudioSource warningAlarmAudio;

    [Header("Story Bounds")]
    public float storyBound = 200f;
    public float radarBound = 75f;

    [Header("Movement")]
    public float minMoveDelay = 2f;
    public float maxMoveDelay = 10f;
    [Tooltip("Max move delay when within threat distance 25.")]
    public float maxMoveDelayClose = 5f;
    [Tooltip("Max distance toward (0,0) per axis each step.")]
    public float maxInwardStep = 10f;
    [Tooltip("Max inward step per axis when within threat distance 50.")]
    public float maxInwardStepClose = 20f;
    [Tooltip("Max distance away from (0,0) per axis each step.")]
    public float maxOutwardStep = 5f;

    [Tooltip("Game over only when BOTH story X and Y are within [-limit, +limit].")]
    public float gotchaAxisLimit = 5f;

    static readonly float[] WarningMarks = { 75f, 50f, 25f };

    Vector2 storyPosition;
    bool stopped;
    float previousThreatDistance = float.MaxValue;
    Animator enemyDotAnimator;

    void Start()
    {
        if (enemyDot != null)
            enemyDotAnimator = enemyDot.GetComponent<Animator>();

        storyPosition = PickStartOutsideRadar();
        previousThreatDistance = ThreatDistance();
        RefreshDotVisibility();
        UpdateAudio();
        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (!stopped)
        {
            float maxDelay = ThreatDistance() <= 25f ? maxMoveDelayClose : maxMoveDelay;
            yield return new WaitForSeconds(Random.Range(minMoveDelay, maxDelay));
            if (stopped) yield break;

            StepAlongRandomPath();
            ClampToStoryBounds();
            RefreshDotVisibility();
            UpdateAudio();

            if (HasReachedOrigin())
            {
                stopped = true;
                StopAllEntityAudio();
                Debug.Log(
                    $"Gotcha! Game over! story=({storyPosition.x:F2}, {storyPosition.y:F2}) " +
                    $"requires both axes in [{-gotchaAxisLimit}, {gotchaAxisLimit}]");
                yield break;
            }
        }
    }

    void StepAlongRandomPath()
    {
        // Per axis: positive = toward origin, negative = away.
        // Within threat 50, allow a larger inward step so it closes in faster.
        float inward = ThreatDistance() <= 50f ? maxInwardStepClose : maxInwardStep;
        storyPosition.x += AxisStep(storyPosition.x, inward);
        storyPosition.y += AxisStep(storyPosition.y, inward);
        Debug.Log($"Entity moved to story position {storyPosition}");
    }

    float AxisStep(float coord, float maxInward)
    {
        if (Mathf.Abs(coord) <= Mathf.Epsilon)
            return 0f;

        float towardOrigin = -Mathf.Sign(coord);
        float step = Random.Range(-maxOutwardStep, maxInward);
        return towardOrigin * step;
    }

    void ClampToStoryBounds()
    {
        storyPosition.x = Mathf.Clamp(storyPosition.x, -storyBound, storyBound);
        storyPosition.y = Mathf.Clamp(storyPosition.y, -storyBound, storyBound);
    }

    bool HasReachedOrigin()
    {
        // Both axes must independently be inside [-gotchaAxisLimit, +gotchaAxisLimit].
        bool xInRange = storyPosition.x >= -gotchaAxisLimit && storyPosition.x <= gotchaAxisLimit;
        bool yInRange = storyPosition.y >= -gotchaAxisLimit && storyPosition.y <= gotchaAxisLimit;
        return xInRange && yInRange;
    }

    bool IsInsideRadarBox()
    {
        return Mathf.Abs(storyPosition.x) <= radarBound
            && Mathf.Abs(storyPosition.y) <= radarBound;
    }

    // Chebyshev distance — matches the square radar rings at 75 / 50 / 25.
    float ThreatDistance()
    {
        return Mathf.Max(Mathf.Abs(storyPosition.x), Mathf.Abs(storyPosition.y));
    }

    void UpdateAudio()
    {
        bool inRadar = IsInsideRadarBox();
        if (inRadarRangeAudio != null)
        {
            if (inRadar)
            {
                inRadarRangeAudio.volume = InRadarRangeVolume(ThreatDistance());
                if (!inRadarRangeAudio.isPlaying)
                {
                    inRadarRangeAudio.loop = true;
                    inRadarRangeAudio.Play();
                }
            }
            else if (inRadarRangeAudio.isPlaying)
            {
                inRadarRangeAudio.Stop();
            }
        }

        float threat = ThreatDistance();
        UpdateWarningAlarm(previousThreatDistance, threat);
        previousThreatDistance = threat;
    }

    // Full volume at threat <= 25; fades to quiet at the radar edge (75).
    float InRadarRangeVolume(float threatDistance)
    {
        if (threatDistance <= 25f)
            return 1f;

        float t = Mathf.InverseLerp(25f, radarBound, threatDistance);
        return Mathf.Lerp(1f, 0.05f, t);
    }

    void UpdateWarningAlarm(float previousThreat, float currentThreat)
    {
        if (warningAlarmAudio == null) return;

        bool insideInnerRing = currentThreat <= 25f;

        if (insideInnerRing)
        {
            // At 25 and within: keep the alarm looping.
            warningAlarmAudio.loop = true;
            if (!warningAlarmAudio.isPlaying)
                warningAlarmAudio.Play();
            return;
        }

        // Left the inner ring — stop continuous alarm.
        if (warningAlarmAudio.loop)
        {
            warningAlarmAudio.loop = false;
            warningAlarmAudio.Stop();
        }

        // One-shot when crossing 75, 50, or 25 inward.
        foreach (float mark in WarningMarks)
        {
            if (previousThreat > mark && currentThreat <= mark)
            {
                warningAlarmAudio.loop = false;
                warningAlarmAudio.Play();
                break;
            }
        }
    }

    void StopAllEntityAudio()
    {
        if (inRadarRangeAudio != null && inRadarRangeAudio.isPlaying)
            inRadarRangeAudio.Stop();

        if (warningAlarmAudio != null)
        {
            warningAlarmAudio.loop = false;
            if (warningAlarmAudio.isPlaying)
                warningAlarmAudio.Stop();
        }
    }

    void RefreshDotVisibility()
    {
        if (enemyDot == null) return;

        bool visible = IsInsideRadarBox();
        if (enemyDot.activeSelf != visible)
            enemyDot.SetActive(visible);

        if (visible)
        {
            enemyDot.transform.position = StoryToRadarWorld(storyPosition);
            UpdateDotColor();
        }
    }

    void UpdateDotColor()
    {
        if (enemyDotAnimator == null) return;
        enemyDotAnimator.SetBool("Danger", ThreatDistance() <= 25f);
    }

    Vector3 StoryToRadarWorld(Vector2 story)
    {
        float halfH = radarCamera.orthographicSize;
        float halfW = halfH * radarCamera.aspect;
        Vector3 cam = radarCamera.transform.position;
        float wx = cam.x + (story.x / radarBound) * halfW;
        float wy = cam.y + (story.y / radarBound) * halfH;
        float wz = enemyDot != null ? enemyDot.transform.position.z : 0f;
        return new Vector3(wx, wy, wz);
    }

    Vector2 PickStartOutsideRadar()
    {
        // Random point in story bounds that is outside the radar box.
        for (int i = 0; i < 64; i++)
        {
            Vector2 candidate = new Vector2(
                Random.Range(-storyBound, storyBound),
                Random.Range(-storyBound, storyBound));

            if (Mathf.Abs(candidate.x) > radarBound || Mathf.Abs(candidate.y) > radarBound)
                return candidate;
        }

        // Fallback: force outside on X.
        return new Vector2(
            Random.Range(radarBound + 1f, storyBound) * (Random.value < 0.5f ? -1f : 1f),
            Random.Range(-storyBound, storyBound));
    }
}
