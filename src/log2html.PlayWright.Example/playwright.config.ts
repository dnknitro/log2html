import { defineConfig, devices } from '@playwright/test'

export default defineConfig({
  testDir: './tests',
  outputDir: './test-results',
  fullyParallel: true,
  retries: 0,
  workers: 2,
  reporter: [
    ['list'],
    ['./reporters/log2html-reporter.ts', {
      environment: 'DEV',
      reportName: 'log2html.PlayWright.Example Execution Report',
      stepCategories: ['test.step', 'expect', 'pw:api']
    }]
  ],
  use: {
    screenshot: 'only-on-failure',
    trace: 'on-first-retry'
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] }
    }
  ]
})
