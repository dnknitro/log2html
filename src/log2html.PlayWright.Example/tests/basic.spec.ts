import { expect, test } from '../src/log2html'

test.describe.configure({ mode: 'parallel' })

test('BasicTestFixture style Playwright report', async ({ page, report }) => {
  await test.step('Arrange log2html records', async () => {
    await report.info('Playwright: Lorem ipsum dolor sit amet, <b>consectetur</b> adipisicing elit,<br/>sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.')

    await test.step('Log JSON payload', async () => {
      await report.json([
        {
          FirstName: 'Ada',
          LastName: 'Lovelace',
          FullName: 'Ada Lovelace',
          Email: 'ada.lovelace@example.test',
          Company: {
            Name: 'Analytical Engines',
            CatchPhrase: 'Executable notes for deterministic examples'
          }
        },
        {
          FirstName: 'Grace',
          LastName: 'Hopper',
          FullName: 'Grace Hopper',
          Email: 'grace.hopper@example.test',
          Company: {
            Name: 'Compiler Labs',
            CatchPhrase: 'Human-readable diagnostics'
          }
        }
      ])
    })
  })

  await test.step('Render browser page', async () => {
    await page.setContent(`
      <main>
        <h1>log2html Playwright Example</h1>
        <p>Custom records are written into the log2html HTML template.</p>
        <label for="first-name">First name</label>
        <input id="first-name" />
        <button type="button" onclick="document.querySelector('#first-name-result').textContent = document.querySelector('#first-name').value">
          Save first name
        </button>
        <output id="first-name-result"></output>
      </main>
    `)
  })

  await test.step('Interact with browser page', async () => {
    await page.getByLabel('First name').fill('Ada')
    await page.getByRole('button', { name: 'Save first name' }).click()
  })

  await test.step('Assert browser page', async () => {
    await expect(page.getByRole('heading', { name: 'log2html Playwright Example' })).toBeVisible()
    await expect(page.locator('#first-name-result')).toHaveText('Ada')
  })

  await report.pass('No Fails!')
})

test('Second UI test keeps report records separate', async ({ page, report }) => {
  await test.step('Arrange second test records', async () => {
    await report.info('Second UI test: each Playwright test should produce its own log2html summary row.')

    await test.step('Log second JSON payload', async () => {
      await report.json({
        testName: 'Second UI test keeps report records separate',
        expectedGrouping: 'Records should be grouped by TestCaseName',
        markers: ['second-test-info', 'second-test-json', 'second-test-pass']
      })
    })
  })

  await test.step('Render second browser page', async () => {
    await page.setContent(`
      <main>
        <h1>Second log2html Playwright Example</h1>
        <p>This test runs in parallel with the first UI test.</p>
        <label for="status">Status</label>
        <input id="status" />
        <button type="button" onclick="document.querySelector('#status-result').textContent = document.querySelector('#status').value">
          Save status
        </button>
        <output id="status-result"></output>
      </main>
    `)
  })

  await test.step('Interact with second browser page', async () => {
    await page.getByLabel('Status').fill('ready')
    await page.getByRole('button', { name: 'Save status' }).click()
  })

  await test.step('Assert second browser page', async () => {
    await expect(page.getByRole('heading', { name: 'Second log2html Playwright Example' })).toBeVisible()
    await expect(page.locator('#status-result')).toHaveText('ready')
  })

  await report.pass('Second UI test passed and stayed separate.')
})
