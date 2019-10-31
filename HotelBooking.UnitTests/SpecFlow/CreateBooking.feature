Feature: CreateBooking
	In order to create booking
	I want it to check if the dates are available
	
Scenario: Test case 1
	Given The start date is 9
	And The end date is 9
	When I press book
	Then the booking result should be success

Scenario: Test case 2
	Given The start date is 9
	And The end date is 21
	When I press book
	Then the booking result should be failure

Scenario: Test case 3
	Given The start date is 21
	And The end date is 21
	When I press book
	Then the booking result should be success

Scenario: Test case 4
	Given The start date is 9
	And The end date is 10
	When I press book
	Then the booking result should be failure

Scenario: Test case 5
	Given The start date is 9
	And The end date is 20
	When I press book
	Then the booking result should be failure

Scenario: Test case 6
	Given The start date is 10
	And The end date is 21
	When I press book
	Then the booking result should be failure

Scenario: Test case 7
	Given The start date is 20
	And The end date is 21
	When I press book
	Then the booking result should be failure

Scenario: Test case 8
	Given The start date is 10
	And The end date is 10
	When I press book
	Then the booking result should be failure

Scenario: Test case 9
	Given The start date is 10
	And The end date is 20
	When I press book
	Then the booking result should be failure

Scenario: Test case 10
	Given The start date is 20
	And The end date is 20
	When I press book
	Then the booking result should be failure