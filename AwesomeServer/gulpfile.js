var gulp = require('gulp');
var clean = require('gulp-clean');
var cssmin = require('gulp-cssmin');
var rename = require('gulp-rename');
var sync = require('gulp-sync')(gulp);
var cfg = require('./gulp/config');

// Clean vendor libraries and minified files
gulp.task('clean', function () {
  return gulp.src(cfg.cleanDirectories)
    .pipe(clean());
});

// Copy original vendor libraries
gulp.task('copy-vendor-source', function () {
  return gulp.src(cfg.vendors.source)
    .pipe(gulp.dest(cfg.directories.web + cfg.directories.vendor));
});

// Copy minified vendor libraries
gulp.task('copy-vendor-min', function () {
  return gulp.src(cfg.vendors.min)
    .pipe(gulp.dest(cfg.directories.web + cfg.directories.vendor + cfg.directories.min));
});

// Copy fonts for font-awesome library
gulp.task('copy-fonts', function () {
  return gulp.src(cfg.vendors.fonts)
    .pipe(gulp.dest(cfg.directories.web + cfg.directories.fonts));
});

// Copy minified custom css
gulp.task('minify-css', function () {
  return gulp.src(cfg.directories.web + cfg.directories.css + '*.css')
    .pipe(cssmin())
    .pipe(rename({ suffix: '.min' }))
    .pipe(gulp.dest(cfg.directories.web + cfg.directories.css + cfg.directories.min));
});

// Default gulp task executes synchronously all previous tasks (so that the cleaning has 
// finished before any copy is started) 
gulp.task('default', sync.sync(
  [
    'clean',
    'copy-vendor-source',
    'copy-vendor-min',
    'copy-fonts',
    'minify-css'
  ]));