# log2html Playwright Example

This project demonstrates a custom Playwright reporter that writes log records into the existing `log2html` HTML report template.

Run it from this folder:

```powershell
npm install
npm test
```

The generated report is written to `../Results/` with a dynamic filename based on the report start time, name, and environment:

```text
yyyy-MM-dd HH.mm.ss log2html.PlayWright.Example Execution Report DEV.html
```

The example UI tests run in parallel and use `report.info(...)`, `report.json(...)`, and `report.pass(...)` records similar to `log2html.Test/BasicTestFixture.cs`. Nested Playwright `test.step(...)` and `expect(...)` steps are flattened into normal log2html detail rows by the custom reporter. Playwright API steps such as `fill`, `click`, and locator assertions are captured from `pw:api` steps as `DEBUG` rows. The resulting report should show one summary row per Playwright test, with records split by `TestCaseName`.
