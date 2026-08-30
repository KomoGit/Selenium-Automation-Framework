@ui
Feature: Contact Form

  Scenario: Submit contact form successfully happy path
    Given I am on the contact page
    When I fill out the contact form with first name "John", last name "Doe", email "john.doe@example.com", subject "Customer service", and message "I am writing to inquire about the warranty policy for the power tools I recently purchased."
    And I submit the contact form
    Then I should see the contact success message "Thanks for your message! We will contact you shortly."

  Scenario: Negative test - invalid email format
    Given I am on the contact page
    When I enter first name "John"
    And I enter last name "Doe"
    And I enter email "invalid-email-format"
    And I select subject "Payments"
    And I enter message "This is a detailed enquiry message regarding payment processing and invoicing terms."
    And I submit the contact form
    Then I should see validation error "Email format is invalid" for "email" field

  Scenario: Negative test - message shorter than 50 characters
    Given I am on the contact page
    When I enter first name "John"
    And I enter last name "Doe"
    And I enter email "john.doe@example.com"
    And I select subject "Warranty"
    And I enter message "Short msg"
    And I submit the contact form
    Then I should see validation error "Message must be minimal 50 characters" for "message" field

  Scenario Outline: Negative test - required validation for empty fields
    Given I am on the contact page
    When I submit the contact form
    Then I should see validation error "<expected_error>" for "<field>" field

    Examples:
      | field      | expected_error          |
      | first_name | First name is required  |
      | last_name  | Last name is required   |
      | email      | Email is required       |
      | subject    | Subject is required     |
      | message    | Message is required     |
