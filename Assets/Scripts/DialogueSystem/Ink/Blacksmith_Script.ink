Blacksmith:Hey lad! I'm the blacksmith, how can I help ya!!
    +[I have some questions.] ->intro

===intro===
{name: Reginald|Blacksmith}:Ask away!
    *[Do you have a name or is it just blacksmith?] -> name
    *[Where are we?] -> where
    *[Why are you here?] -> why
    +[See you later.] -> ending

===name===
Blacksmith:My name? It's been a while let me think...
Reginald:I believe that my name is Reginald.
    +[You believe?]
        Reginald: My memory is quite fuzzy right now. I don't remember much before I awoke in this place.
-> intro

===where===
{name: Reginald|Blacksmith}:Hell if I know, I've been wondering that myself.
{name: Reginald|Blacksmith}:I was actually hoping you would know.
-> intro

===why===
{name: Reginald|Blacksmith}:I haven't the foggiest idea.
{name: Reginald|Blacksmith}:My theory is that were being used for some kind of test.
-> intro

===ending===
{name: Reginald|Blacksmith}:See you later lad!
->END