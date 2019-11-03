Feature: FullyOccupiedDates
	I want to get list of fully occupied dates for the date range I specify

Scenario: 1 booking, 11 days
	Given A booking between 10 and 20 exists
	When I get fully occupied dates
	Then the result should have 11 fully occupied days

Scenario: 1 booking, 15 days
	Given A booking between 10 and 24 exists
	When I get fully occupied dates
	Then the result should have 15 fully occupied days
