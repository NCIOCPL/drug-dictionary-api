Feature: Pretty URLs which don't match anything result in an HTTP 404 status and a friendly message.

    Background:
        * url apiHost

    Scenario Outline: Non-existant pretty URL name.
        Test for name '<prettyName>'

        Given path 'Drugs', prettyName
        When method get
        Then status 404
        And match response.Message == "No result found."

        Examples:
            | prettyName         |
            | chicken            |
            | autologous-ic9-gd2 |
            | egfr antisense dna |
            | BENZOYLPHENYLUREA  |
            | s-3304             |
