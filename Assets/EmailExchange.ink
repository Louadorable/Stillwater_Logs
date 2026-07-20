==default==
# compose_on:true
-> DONE

== wait_2sec ==
# time:2
# compose_on:false
# speaker:_
-> DONE

== wait_5sec ==
# time:5
# compose_on:false
# speaker:_
-> DONE

== wait_10sec ==
# time:10
# compose_on:false
# speaker:_
-> DONE


== email1 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email2
Good Evening, 

As of: 21:00, 22nd of April, the Stillwater entity has awakened.

I will attempt to weather its storm and provide further updates from my site here Hopefully the narcotics aboard this vessel are still in date, and or have been replaced relatively recently. If not, I hope whichever Ethics Committee reads this down the line gives our External Affairs Department a good kicking.

Right, I best go get some needles prepped. Can't be awake when this thing is near.

Please pass this along to the relevant parties. Thank you, 

SW #stop
-> wait_5sec

== email2 ==
# speaker:MorriganSite_AutomatedReply@gov.org
# title:AUTOMATED - PLEASE DO NOT RESPOND TO THIS MESSAGE
# incomingEmail:true
# next_knot:email3
# compose_on:false

Thank you Stillwater for messaging Command Site Morrigan.

A reminder to:

1) Always maintain secrecy when dealing with the general population. 
  
2) When consensus reality is violated directly in front of the general population:

. Take them in for amnesticisation.

. Shoot them (if amnesics are not able to be administered in 48 hours)

     
3) Even if Civil Command Codes are known to External Affairs Department Operatives. ASK for permission to use them from SITE COMMAND. 

Thank you.#stop

-> wait_5sec


== email3 ==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:RE Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:email4
# compose_on:true
# repliable:true
Hey dude, 

Look, can you forward this yourself? I’m not working right now, k?

boss pissed me off. 

Bye.#stop

-> DONE

==email4==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email5

Evening, 

Who is this? 

Considering the situation, I don’t really have the time right now to do extensive paperwork. That’s your job, isn’t it? 

Thank you.

SW
 #stop

-> wait_5sec

==email5==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:RE Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:choice1
# repliable:true
# compose_on:true

my life is at risk! pff! yeah sure..

look, that's what they said to me when I decided to tell the dudes over on the WAR ThHUNDER forumns that we've got weapons in here from some higher-dimensional reality that’s way better than anything their countries could ever build.

boss got really PISSED. Said I was a nepo-baby intern - WHICH I AM NOT. 

so, I'm quiet quittin. Using my workers rights as I should. I'll get everything I want out of this place and leave at the end of summer. 

you wanna be part of the child-eating 1% too? Then fuck off. #stop

-> DONE


==choice1==
# speaker:StillWater_MoitoringStation@gov.org
# choice:_

+ Option 1) Fight it and Speak your mind.
-> email6_A
+ Option 2) Convince him you are also a slacker and just want someone else to deal with this.
-> email6_B

== email6_A ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email7_A

Giovanni, I understand you are upset. But you cannot behave like this in any professional working environment. 

I know the higher-ups can be stern. But they are like that for good reason. People’s lives are at risk. My life is at risk right now.

How would you feel in my position? Trapped in the middle of nowhere while back-up is being held up by some dismissive intern? 

All you have to do is forward this email to the Site Director and other relevant departments. I'll be out of your hair entirely once you've done that. 

Thanks,

SW #stop

-> wait_5sec


== email6_B ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email7_B

Hey, look I get it. Management are twats on a power-trip aren't they?

I've been stuck out here for a year, and they still haven't told me when I can stand down and come home. I'm constantly covered in itchy mosquito bites and have to cut my own wood for my logfire here.

Look, I am about as done with all this as you are. 

Why don't we be comrades in arms and dump this on someone else, hey? 

Help me out, Giovanni.

SW #stop

-> wait_2sec


==email7_A==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:RE Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:Email8_B
fuck off. #stop

-> wait_10sec

==email7_B==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:RE Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:email8_A

site director is off duty. just getting automated replies back.

saw the lights were still on in the anomalous research department. I'll pass you onto somebody there. #stop

-> wait_10sec


== email8_A==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:Need Your Data
# incomingEmail:true
# next_knot:Email9
# compose_on:true
# repliable:true

Hey Stillwater,

Just got your message. It's up and at it then?

Shame it had to be in the middle of the night like this. You're going to struggle to drag anyone back into the office. 

I'll try reaching out to my department chief. Hopefully, they’ll be willing to pass this up to the Site Director. Tried contacting him myself, but just got an automated reply back talking about how we should forward this to his secretary… who’s currently on paternity leave.

Okay, aside from that. If we’re the only two working on this right now, we probably should make sure that thing’s going near civilisation any time soon. We all know what happened when it last reared its ugly head.

Could you send me some coords? Probably only need to plot three of them for me. 

We’ve got a simulator here that should be able to predict where this entity will move next. 

Typically, it’s somewhere where there is a large amassing of electrical signal, be it tech or a vast quantity of human brain activity. Now, hopefully, this thing will just be bee-lining for our data centres out there, but you never know.

Also, ping the Site Morrigan ‘Mobile Task Force’ SM-MTF. Tell them it’s out.

They’ll be quicker to pick up than the Site Director. Might even get his attention quicker than us.

So:

Send me three coords
Contact SM-MTF

Good luck out there, Stillwater.

Dr Marly Finch #stop


->DONE



== Email8_B ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:Incident Report
# incomingEmail:true
# next_knot:Email8_C 
# repliable:true

Stillwater, can you respond?

An incident report just came through. Some nutter out there is causing chaos.

You are the closest to all of it. Can you see anything?

Kind Regards

Dr Marly Finch #stop

->DONE


== Email8_C ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Incident Report
# playerEmail:true
# next_knot:email7_A

Hi,

I don't believe that person is a person attacking our infrastructure. The Entity here has awoken. 

Would appreciate some help. I have been struggling to contact anyone higher up the chain of command.

Thanks,

SW #stop

->wait_5sec


== Email9 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Need Your Data
# playerEmail:true
# next_knot:MTF_Prompt
# compose_on:true

Hello, Doctor. 

Here is the coordinates I have tracked. 

(Player Inserts here)
(Player Inserts here)
(Player Inserts here)


Let me know if you hear back from the Site Director. 

-SW #stop

->wait_2sec

== MTF_Prompt ==
# speaker:SM-MTF@gov.org
# title:A REMINDER - SM MTF
# incomingEmail:true
# next_knot:Email10
# repliable:true
# compose_on:true
If a situation is escalating out of control, don’t contact your supervisor. 

Contact us.

Site Morrigan’s Mobile Task Force squad is here to serve you. 

Email: SM-MTF@gov.org
Phone Number (Internal): 111
Phone Number (External): +44 4852 612 #stop

-> DONE

== Email10 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:Requesting EVAC from Stillwater Monitoring Station
# playerEmail:true
# next_knot:Email11

Greetings, 

I am the watcher of the Stillwater Monitoring Station. 

At 21:00, 22nd of April - My equipment detected the Stillwater Entity had awakened.

Currently dealing with it as best I can. Can you send some people down here to provide me EVAC? As well as put this thing back to sleep?

Thanks, 

SW #stop

->wait_5sec

== Email11 ==
# speaker:SM-MTF@gov.org
# title:RE Requesting EVAC from Stillwater Monitoring Station
# incomingEmail:true
# next_knot:Email12
# repliable:true
# compose_on:true

Good Evening, 

I am afraid to say you’ll need to hunker down for some time. Our main body of soldiers has been sent to assist the Site Hermes staff. A containment breach has occurred over there.

Perhaps send a request to MTF “Sioux”. They are the next closest available force.

Until then you’ll be shortlisted on our deployment list.

Good luck,

SM-MTF #stop

-> DONE

== Email12 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Requesting EVAC from Stillwater Monitoring Station
# playerEmail:true
# next_knot:Email13

Hello MTF Sioux, 

I am the watcher of Monitoring Station Stillwater.

As of 21:00 tonight the Stillwater Entity awoke from its slumber.

Currently dealing with it as best I can. Can you send some people down here to provide me EVAC?

I have already contacted my local site’s MTF, but they are busy dealing with a containment breach. I have forwarded our email correspondence to you.

Really could do with some help. Quickly please.

SW #stop

->wait_5sec

== Email13 ==
# speaker:SS-MTF@gov.org
# title:RE Requesting EVAC from Stillwater Monitoring Station
# incomingEmail:true
# next_knot:Email14
# compose_on:false

Hello Stillwater,

Yes, we can provide assistance to you. However, due to your geographical location and poor weather conditions, we expect it will take around 12 hours to reach you.

Please remain calm and focus on your objectives until then.

SS-MTF #stop

->wait_5sec

== Email14 ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:RE Need Your Data
# incomingEmail:true
# next_knot:Email15
# repliable:true
# compose_on:true

Evening Stillwater, 

Look, I just ran the simulation. It predicts with a 72% certainty that the entity is going to go after the town of Brandsback within the next few hours. It’s just going on it’s.. ‘Morning strut’ right now. Flex it’s muscles before it runs the marathon. 

Maybe if the MTF makes it to your position, they can shirk it off course. 

Still trying to contact the site director. Having no luck. Think my department chief’s already asleep.

Dr Marly Finch #stop

-> DONE

== Email15 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Need Your Data
# playerEmail:true
# next_knot:Email16

Hello Doctor, 

Our Task Force is busy right now, and had to call one in from further afield. 

When did your simulation say that thing was going to change course?

SW #stop

->wait_2sec

== Email16 ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:RE Need Your Data
# incomingEmail:true
# next_knot:Email17
# repliable:true
# compose_on:true

Hello Stillwater, 

It said it was going to change course in around 2 hours. #stop

->DONE

== Email17 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Need Your Data
# playerEmail:true
# next_knot:Email18

We have a problem then. The new MTF is going to take 12 hours to reach here. #stop

->wait_2sec

== Email18 ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:RE Need Your Data
# incomingEmail:true
# next_knot:Email19
# repliable:true
# compose_on:true

Shit. 

We’ve got to call in an evacuation of Brandsback then. 

I doubt you're any higher of a rank than me? We’re both around level 2-4 right? 

The Site Director is the only person probably high enough to grant permission to use the civil command codes. #stop

->DONE


== Email19 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Need Your Data
# playerEmail:true
# next_knot:Email20
Well, I’ve got some here. Stillwater is part of the External Affairs Department. Can’t tell you them obviously. But I cannot use them without clearance. #stop

-> wait_5sec

== Email20 ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:RE Need Your Data
# incomingEmail:true
# next_knot:Email21
# repliable:true
# compose_on:true

You have got to use them, Stillwater!

Sure, the organisation will be annoyed that you broke protocol. However, I am certain an Ethics Committee will clear you of any wrong doing, after the fact. You were just doing what was right in the moment. 

Look, if anything, the hammer will come down hardest on our site director for being so hard to contact. Not you. #stop

-> DONE


== Email21 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Need Your Data
# playerEmail:true
# next_knot:Email22
And what if our site chief worms his way out of this? Directs the blame onto me?
I am not risking my career over this! Hell, my life if they really get pissy!
 #stop

->wait_2sec


== Email22 ==
# speaker:MorriganSite_AR_MarlyFinch@gov.org
# title:RE Need Your Data
# incomingEmail:true
# next_knot:Email21
# repliable:false
# compose_on:false

Look, if you want to bring this up with him yourself. Here’s his radio frequency. 166 MHz.

Found it kicking around in some memos. 

Not been having any luck myself getting his attention. Might be because this building I am in is shielded. 

You out in the open air though? You may have more luck.

And if he doesn’t respond? Just send you codes to the nearest news agency. You know the drill, I assume? #stop

-> DONE