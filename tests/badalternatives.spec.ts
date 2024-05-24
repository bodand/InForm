import { test, expect } from '@playwright/test';

//creates an empty test and tries to publish it
test('empty everyhing', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('The form title must be between 4 and 64 characters');
  await expect(page.locator('form')).toContainText('At least one form element is required');
});

//creates a new test without element and tries to publish it
test('no element', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('At least one form element is required');
});
//creates a new test and only fills out the default information, leaves the elements information empty and tries to publish it
test('empty element fields', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('A title is required');
});
//creates a form with 2 elements text element, multi-choice element fills out everything, leaves the maximum selectable at 0
test('multi-choice 0 selected', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.locator('#str_elem_title').click();
  await page.locator('#str_elem_title').fill('title');
  await page.locator('#str_elem_subtitle').click();
  await page.locator('#str_elem_subtitle').fill('subtitle');
  await page.locator('#mc_elem_title').click();
  await page.locator('#mc_elem_title').fill('title');
  await page.locator('#mc_elem_subtitle').click();
  await page.locator('#mc_elem_subtitle').fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('The minimum of 1 must be selected for maximum');
});


//the maximum selectable should be at maximum the number of options
//creates a multichioce whith less option then the number in the maximum selectable field
test('more anwser then option', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('titel');
  await page.getByLabel('Form Title').press('Tab');
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Form Subtitle').press('Tab');
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element Text element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByLabel('Option 0').click();
  await page.getByLabel('Option 0').fill('opt1');
  await page.getByLabel('Option 1').click();
  await page.getByLabel('Option 1').fill('opt2');
  await page.getByLabel('Maximum selectable').click();
  await page.getByLabel('Maximum selectable').fill('3');
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.getByRole('heading')).toContainText('Form elements');
});


//creates a multi-choice test, fills the maximum selectable field with negative number
test('minus maximum option number', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByLabel('Option').click();
  await page.getByLabel('Option').fill('opt1');
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByLabel('Maximum selectable').click();
  await page.getByLabel('Maximum selectable').fill('-1');
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('The minimum of 1 must be selected for maximum');
});

//creates a test with 2 element then deletes them then tries to publish it
test('delete element', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.getByRole('button', { name: 'Delete element' }).first().click();
  await page.getByRole('button', { name: 'Delete element' }).click();
  await page.getByRole('button', { name: 'Publish' }).click();
  await expect(page.locator('form')).toContainText('At least one form element is required');
});

//creates a multi-choice test and sets the maximum selectable field 1, then tries to select multiple
test('multi option 1 select', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.locator('#str_elem_title').click();
  await page.locator('#str_elem_title').fill('title');
  await page.locator('#str_elem_subtitle').click();
  await page.locator('#str_elem_subtitle').fill('subtitle');
  await page.locator('#mc_elem_title').click();
  await page.locator('#mc_elem_title').fill('title');
  await page.locator('#mc_elem_subtitle').click();
  await page.locator('#mc_elem_subtitle').fill('subtitle');
  await page.getByLabel('Maximum selectable').click();
  await page.getByLabel('Maximum selectable').fill('1');
  await page.getByText('Add option').click();
  await page.getByText('Add option').click();
  await page.getByLabel('Option 0').click();
  await page.getByLabel('Option 0').fill('opt1');
  await page.getByLabel('Option 1').click();
  await page.getByLabel('Option 1').fill('opt2');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('opt1').check();
  await page.getByLabel('opt2').check();
  await expect(page.getByLabel('opt1')).not.toBeChecked();
});

//create a test then trying to check answers with wrong answer
test('wrong password', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('password');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response');
  await page.getByRole('button', { name: 'Submit' }).click();
  await page.getByRole('button', { name: 'See results' }).click();
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('pass');
  await page.getByRole('button', { name: 'Open' }).click();
  await expect(page.locator('form')).toContainText('Incorrect password.');
});


//fill required is enabled, however user did not filled out
test('fill required', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByLabel('Fill required').check();
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByRole('button', { name: 'Submit' }).click();
  await expect(page.locator('form')).toContainText('This field is required');
});

//check required field at multi-choice
test('required multi-choice', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.getByLabel('Fill required').check();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByRole('button').nth(4).click();
  await page.getByLabel('Option 0').click();
  await page.getByLabel('Option 0').fill('opt1');
  await page.getByLabel('Option 1').click();
  await page.getByLabel('Option 1').fill('opt2');
  await page.getByLabel('Maximum selectable').click();
  await page.getByLabel('Maximum selectable').fill('1');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByRole('button', { name: 'Submit' }).click();
  await expect(page.locator('form')).toContainText('This field is required');
});

