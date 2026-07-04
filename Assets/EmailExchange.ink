==default==

->DONE

== wait_5sec ==
# time:5
# speaker:_
-> DONE

== wait_10sec ==
# time:10
-> DONE

==choice1==
# speaker:StillWater_MoitoringStation@gov.org
# choice:_

+ Boop
->email1
+ Gayin
->email1


== email1 ==
# speaker:StillWater_MoitoringStation@gov.org
# title:Issue
# playerEmail:true
Test, whee woo!
Papa
# next_knot:email2
-> wait_5sec 

== email2 ==
# speaker:MorriganSite_AutomatedReply@gov.org
# title:Problem
# incomingEmail:true
AUTOMATED - PLEASE DO NOT RESPOND TO THIS MESSAGE

Thank you Stillwater for messaging Command Site Morrigan.

A reminder to:

1) Always maintain secrecy when dealing with the general population. 

2) When consensus reality is violated directly in front of the general population:

- Take them in for amnesticisation.

- Shoot them (if amnesics are not able to be administered in 48 hours)

3) Even if Civil Command Codes are known to External Affairs Department Operatives. ASK for permission to use them from SITE COMMAND. 

Thank you.


->DONE
