@ui
Feature: Sidebar Navigation

  Scenario: Verify search functionality successfully happy path
    Given I am on the home page
    When I fill out the search bar with "Pliers"
    And I click the search button
    Then I should see the search results for "Pliers"
    