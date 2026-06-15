import tseslint from '@typescript-eslint/eslint-plugin'
import tsParser from '@typescript-eslint/parser'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'

export default [
	{
		ignores: ['dist', '.tmp-vite-build'],
	},
	...tseslint.configs['flat/recommended'],
	reactHooks.configs.flat.recommended,
	reactRefresh.configs.vite,
	{
		files: ['**/*.{ts,tsx}'],
		languageOptions: {
			parser: tsParser,
			ecmaVersion: 2020,
			sourceType: 'module',
		},
		rules: {
			'@typescript-eslint/no-unused-vars': 'warn',
			'react-refresh/only-export-components': [
				'warn',
				{ allowConstantExport: true },
			],
			'semi': ['warn', 'never'],
		},
	},
]
