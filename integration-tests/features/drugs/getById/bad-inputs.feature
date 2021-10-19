Feature: Submitting an incorrect ID to GetById results in a suitable HTTP status and message.

    Background:
        * url apiHost

    Scenario Outline: Non-positive IDs result in a 400 status code.

        Given path 'Drugs', id
        When method get
        Then status 400
        And match response.Message == 'Not a valid ID.'

        Examples:
            | id    |
            | -7400 |
            | 0     |


    Scenario Outline: IDs which don't exist in the system result in a 404 status code.
        id = '<id>'

        Given path 'Drugs', id
        When method get
        Then status 404
        And match response.Message == 'No result found.'

        Examples:
            | id         |
            | 100        |
            | 32767      |
            | 8000000000 |
