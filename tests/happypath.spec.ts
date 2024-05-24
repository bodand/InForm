import { test, expect } from '@playwright/test';
// happy path, create a new test fill out the required fields then add a new text element, fill it's fields out, then submits it
test('1 input element', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('form title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('form subtitle');
  await page.getByLabel('Results password').click();
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('element title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('element subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response');
  await page.getByRole('button', { name: 'Submit' }).click();
});
//creates with 2 types of input fields a simple and a multichoice test
test('2 input element', async ({ page }) => {
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
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('stitle');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Multi-choice element' }).click();
  await page.locator('#mc_elem_title').click();
  await page.locator('#mc_elem_title').fill('title');
  await page.locator('#mc_elem_subtitle').click();
  await page.locator('#mc_elem_subtitle').fill('title');
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByRole('button', { name: 'Add option' }).click();
  await page.getByLabel('Option 0').click();
  await page.getByLabel('Option 0').fill('option 1');
  await page.getByLabel('Option 1').click();
  await page.getByLabel('Option 1').fill('option 2');
  await page.getByLabel('Maximum selectable').click();
  await page.getByLabel('Maximum selectable').fill('1');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByRole('button', { name: 'Submit' }).click();

});

//creating a test answer the question then check the answers
test('check answers', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Title').press('Tab');
  await page.getByLabel('Form Subtitle').fill('subtite');
  await page.getByLabel('Form Subtitle').press('Tab');
  await page.getByLabel('Results password').fill('pass');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Title', { exact: true }).press('Tab');
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response');
  await page.getByRole('button', { name: 'Submit' }).click();
  await page.getByRole('button', { name: 'See results' }).click();
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('pass');
  await page.getByRole('button', { name: 'Open' }).click();
  await expect(page.locator('tbody')).toContainText('response');
});




//creates a simple test with 1 simple text element, then opens the quiz and submits an answer
test('answer simple test', async ({ page }) => {
  let  regex =new RegExp("https:\/\/localhost:7211\/form\/(.*)")

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
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByRole('button', { name: 'Submit' }).click();

  let id = await page.url().match(regex);


  await page.getByRole('link', { name: 'InForm' }).click();
  await page.getByLabel('FormID').click();
  await page.getByLabel('FormID').fill(id![1]);
  await page.getByRole('button', { name: 'Open' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response');
  await page.getByRole('button', { name: 'Submit' }).click();
  await expect(page.getByRole('article')).toContainText('Thank you for filling out this form. See results');
});


//creates a test submits an answer, opens it submit another answer check the submitted answers
test('review multiple answers', async ({ page }) => {
  let  regex =new RegExp("https:\/\/localhost:7211\/form\/(.*)")

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
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response1');
  await page.getByRole('button', { name: 'Submit' }).click();


  let id = await page.url().match(regex);
  


  await page.getByRole('link', { name: 'InForm' }).click();
  await page.getByLabel('FormID').click();
  await page.getByLabel('FormID').fill(id![1]);
  await page.getByRole('button', { name: 'Open' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response2');
  await page.getByRole('button', { name: 'Submit' }).click();
  await page.getByRole('button', { name: 'See results' }).click();
  await page.getByLabel('Password').click();
  await page.getByLabel('Password').fill('pass');
  await page.getByRole('button', { name: 'Open' }).click();
  await expect(page.locator('tbody')).toContainText('response1');
  await expect(page.locator('tbody')).toContainText('response2');
});

//when no password given the answers should be available
test('no password', async ({ page }) => {
  await page.goto('https://localhost:7211/');
  await page.getByRole('link', { name: 'Create' }).click();
  await page.getByLabel('Form Title').click();
  await page.getByLabel('Form Title').fill('title');
  await page.getByLabel('Form Subtitle').click();
  await page.getByLabel('Form Subtitle').fill('subtitle');
  await page.getByText('Add element').click();
  await page.getByRole('button', { name: 'Text element' }).click();
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Title', { exact: true }).press('Tab');
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response');
  await page.getByRole('button', { name: 'Submit' }).click();
  await page.getByRole('button', { name: 'See results' }).click();
  await expect(page.locator('tbody')).toContainText('response');
});

//creates a test and tries to give a multi line answer
test('multiline', async ({ page }) => {
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
  await page.getByLabel('Title', { exact: true }).click();
  await page.getByLabel('Title', { exact: true }).fill('title');
  await page.getByLabel('Subtitle', { exact: true }).click();
  await page.getByLabel('Subtitle', { exact: true }).fill('subtitle');
  await page.getByLabel('Multiline').check();
  await page.getByRole('button', { name: 'Publish' }).click();
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response line 1');
  await page.getByLabel('Response').press('Shift+Enter');
  await page.getByLabel('Response').click();
  await page.getByLabel('Response').fill('response line 2');
});