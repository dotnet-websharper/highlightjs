import { build } from 'esbuild'

var options =
{
  entryPoints: ['./build/WebSharper.Highlightjs.Sample.js'],
  bundle: true,
  minify: true,
  format: 'iife',
  outfile: '../dist/Scripts/WebSharper.Highlightjs.Sample.min.js',
  globalName: 'wsbundle'
};

build(options);

