Feature: Autosuggest has a health check endpoint

    Background:
        * url apiHost

    Scenario: The endpoints reports "alive!" when autosuggest is healthy.

        Given path 'autosuggest', 'status'
        When method get
        Then status 200
        And match response == 'alive!'
