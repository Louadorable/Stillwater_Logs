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
Test, whee woo!'
Papa #stop
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

SW #stop

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

SW #stop

-> wait_5sec


== email6_B ==
# speaker:StillWater_MoitoringStation@gov.org
# title:RE Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email7_B

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
# next_knot:_

Greetings, 

I am the watcher of the Stillwater Monitoring Station. 

At 21:00, 22nd of April - My equipment detected the Stillwater Entity had awakened.

Currently dealing with it as best I can. Can you send some people down here to provide me EVAC? As well as put this thing back to sleep?

Thanks, 

SW

->wait_5sec


