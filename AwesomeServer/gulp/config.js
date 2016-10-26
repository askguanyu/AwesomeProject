var directories = require('./config/directories');
module.exports.directories = directories;

var cleanDirectories = require('./config/cleanDirectories');
cleanDirectories = cleanDirectories.map(function (path) {
  path = path.replace('{web}', directories.web);
  path = path.replace('{vendor}', directories.vendor);
  path = path.replace('{fonts}', directories.fonts);
  path = path.replace('{css}', directories.css);
  return path.replace('{min}', directories.min);
});
module.exports.cleanDirectories = cleanDirectories;

var vendors = require('./config/vendors');
vendors.source = vendors.source.map(function (path) {
  return path.replace('{mod}', directories.mod);
});
vendors.min = vendors.min.map(function (path) {
  return path.replace('{mod}', directories.mod);
});
vendors.fonts = vendors.fonts.map(function (path) {
  return path.replace('{mod}', directories.mod);
});
module.exports.vendors = vendors;