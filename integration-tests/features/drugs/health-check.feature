Feature: Drug search has a health check endpoint

    Background:
        * url apiHost

    Scenario: The old drug status endpoint forwards to the new health check.

        * configure followRedirects = false
        Given path 'drugs', 'status'
        When method get
        Then status 301
        And match responseHeaders['Location'][0] == '/healthcheck/status'
