Feature: Autosuggest has a health check endpoint

    Background:
        * url apiHost

    Scenario: The old autosuggest status endpoint forwards to the new health check.

        * configure followRedirects = false
        Given path 'autosuggest', 'status'
        When method get
        Then status 301
        And match responseHeaders['Location'][0] == '/healthcheck/status'