Feature: Expand shows a list of terms starting with the specified letter.

    Background:
        * url apiHost

    Scenario: The special letter # is specified.

        Given path 'Drugs', 'expand', '#'
        And params {from: 0, size: 1000}
        When method get
        Then status 200
        And match each $.results[*].firstLetter == '#'


    Scenario Outline: A letter of the alphabet is specified.
    Letter: '<letter>'

        Given path 'Drugs', 'expand', letter
        And params {from: 0, size: 1000}
        When method get
        Then status 200
        And match each $.results[*].firstLetter == letter
        And match each $.results[*].name[0] == letter

        Examples:
            | letter |
            | a      |
            | b      |
            | c      |
            | d      |
            | e      |
            | f      |
            | g      |
            | h      |
            | i      |
            | j      |
            | k      |
            | l      |
            | m      |
            | n      |
            | o      |
            | p      |
            | q      |
            | r      |
            | s      |
            | t      |
            | u      |
            | v      |
            | w      |
            | x      |
            | y      |
            | z      |
