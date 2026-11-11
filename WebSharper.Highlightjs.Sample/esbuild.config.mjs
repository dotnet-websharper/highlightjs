import { build } from 'esbuild'
import { readFileSync, writeFileSync, copyFileSync } from 'fs'

const name = 'WebSharper.Highlightjs.Sample'

// The WebSharper-generated module imports the npm `highlight.js` package (core,
// the registered languages and the linked stylesheet) plus the WebSharper
// runtime; bundle it all into a single IIFE for the static page. The
// highlight.js style imported by `Styles.AtomOneLight()` is emitted next to the
// script as `<name>.min.css` and linked from index.html.
await build({
  entryPoints: [`./build/${name}.js`],
  bundle: true,
  minify: true,
  format: 'iife',
  outfile: `../dist/Scripts/${name}.min.js`,
  globalName: 'wsbundle'
})

copyFileSync('./site.css', '../dist/site.css')

// Fold the WebSharper head (linked resources) into the page.
const head = readFileSync(`./build/${name}.head.html`, 'utf8')
const html = readFileSync('./index.html', 'utf8').replace('<!-- ws-head -->', head)
writeFileSync('../dist/index.html', html)

console.log('Bundled:', name)
