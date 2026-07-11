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
# compose_on:true
# repliable:true

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
# title:Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email7_A

Giovanni, I understand you are upset. But you cannot behave like this in any professional working environment. 

I know the higher-ups can be stern. But they are like that for good reason. People's lives are at risk. My life is at risk right now.

How would you feel in my position? Trapped in the middle of nowhere while back-up is being held up by some dismissive intern? 

All you have to do is forward this email to the Site Director and other relevant departments. I'll be out of your hair entirely once you’ve done that. 

Thanks,

SW #stop

-> wait_5sec


== email6_B ==
# speaker:StillWater_MoitoringStation@gov.org
# title:Stillwater Entity Has Awoken
# playerEmail:true
# next_knot:email7_B

Hey, look I get it. Management are twats on a power-trip aren’t they?

I've been stuck out here for a year, and they still haven't told me when I can stand down and return home. I'm constantly covered in mosquito bites and have to cut my own wood for my logfire here. 

Look, I am about as done with all this as you are. I want this out of my hair as much as you do. 

Why don't we be comrades in arms and dump this on someone else, hey? 

Help me out, Giovanni.

SW #stop

-> wait_2sec


==email7_A==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:_
fuck off. #stop
-> DONE

==email7_B==
# speaker:MorriganSite_GiovannniHaslo@gov.org
# title:Stillwater Entity Has Awoken
# incomingEmail:true
# next_knot:_

site director is off duty. just getting automated replies back.

saw the lights were still on in the anomalous research department. I'll pass you onto somebody there. #stop

-> DONE