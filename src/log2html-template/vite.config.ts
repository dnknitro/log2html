import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'
import { viteSingleFile } from "vite-plugin-singlefile"

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [react()/*, viteSingleFile()*/],
	base: 'https://dnknitro.github.io/log2html/',
	build: {
		outDir: '../../docs/',
		// sourcemap: true,
		// 	rollupOptions: {
		// 		external: ['react', 'react-dom'],
		// 		output: {
		// 			// Provide global variables to use in the UMD build
		// 			// for externalized deps
		// 			globals: {
		// 				'react': 'React',
		// 				'react-dom': 'ReactDOM',
		// 			},
		// 		},
		// 	},
	},
})
