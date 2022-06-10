Feature: Drug search has a health check endpoint

    Background:
        * url apiHost

    Scenario: The endpoints reports "alive!" when drug search is healthy.

        Given path 'drugs', 'status'
        When method get
        Then status 200
        And match response == 'alive!'
