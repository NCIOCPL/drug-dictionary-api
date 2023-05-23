Feature: Healthcheck

    The system is able to report whether it is in a healthy condition.

    Background:
        * url apiHost

    Scenario: The health check responds "alive!" if the index is loaded and healthy.

        Given path 'HealthCheck', 'status'
        When method get
        Then status 200
        And match response == 'alive!'