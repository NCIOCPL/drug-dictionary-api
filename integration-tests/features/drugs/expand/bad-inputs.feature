Feature: The service responds appropriately to invalid inputs.

    Background:
        * url apiHost

    Scenario Outline: An invalid size is specified and the service falls back to using 100.
    size: '<size>', letter: '<letter>'

        Given path 'Drugs', 'expand', letter
        And params {from: 0, size: <size>}
        When method get
        Then status 200
        And response.results.length == 100

        Examples:
            # NOTE: Letters selected to have at least 100 matches.
            | size | letter |
            | 0    | a      |
            | -5   | r      |
            | -500 | h      |


    Scenario Outline: An invalid from offset is specified and the service falls back to offset 0.
    from: '<from>', letter: '<letter>'

        Given path 'Drugs', 'expand', letter
        And params {from: <from> }
        When method get
        Then status 200
        And match response.meta.from == 0

        Examples:
            | from | letter |
            | -5   | r      |
            | -500 | h      |


    Scenario: A "letter" with more than one character is specified.

        Given path 'Drugs', 'expand', 'chicken'
        When method get
        Then status 400

