Feature: Can retrieve records of only a single resource type.

    Background:
        * url apiHost

    Scenario Outline: Get only records of type DrugAlias

    Given path 'Drugs'
    And params {from: <from>, size: 100, includeResourceTypes: ['DrugAlias']}
    When method get
    Then status 200
    And match response == read(expected)

    Examples:
        | from | expected                               |
        | 0    | resource-types-alias-from-at-0.json    |
        | 1000 | resource-types-alias-from-at-1000.json |
        | 3000 | resource-types-alias-from-at-3000.json |


    Scenario Outline: Get only records of type DrugTerm

    Given path 'Drugs'
    And params {from: <from>, size: 100, includeResourceTypes: ['DrugTerm']}
    When method get
    Then status 200
    And match response == read(expected)

    Examples:
        | from | expected                              |
        | 0    | resource-types-term-from-at-0.json    |
        | 1000 | resource-types-term-from-at-1000.json |
        | 3000 | resource-types-term-from-at-3000.json |