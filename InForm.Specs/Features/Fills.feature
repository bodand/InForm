Feature: Fills

A feature that has scenarios that test the API endpoints provided by the FillsController

@acceptedPostFillData
Scenario: Tries adding data that is accepted by the controller
	Given a fillrequest with data that is accepted by the controller
	When the fillrequest is sent to the controller
	Then the controller should return Accepted

@badRequestPostFillData
Scenario: Tries adding data that is not accepted by the controller given a bad request
	Given a fillrequest with data that is not accepted by the controller
	When the fillrequest is sent to the controller
	Then the controller should return BadRequest

@notFoundPostFillData
Scenario: Tries adding data that is rejected because of non existing form
	Given a fillrequest with data that is rejected because of non existing form
	When the fillrequest is sent to the controller
	Then the controller should return NotFound

@okGetFillData
Scenario: Tries getting data that is accepted by the controller
	Given a getrequest with data that is accepted by the controller
	When the getrequest is sent to the controller
	Then the controller should return Ok

@notFoundGetFillData
Scenario: Tries getting data that is rejected because of non existing form
	Given a getrequest with data that is rejected because of non existing form
	When the getrequest is sent to the controller
	Then the controller should return NotFound for getfilldata

@badRequestGetFillData
Scenario: Tries getting data that is rejected because of bad request
	Given a getrequest with data that is rejected because of bad request
	When the getrequest is sent to the controller
	Then the controller should return BadRequest for getfilldata

@UnauthorizedGetFillData
Scenario: Tries getting data that is rejected because of unauthorized request
	Given a getrequest with data that is rejected because of unauthorized request
	When the getrequest is sent to the controller
	Then the controller should return Unauthorized
