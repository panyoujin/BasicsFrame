require.config({

	paths: {
		angular: 'angular',
		zepto: 'zepto',
		common: 'common',
		wxapi: 'wxapi',
		domready: 'domready'
	},
	shim: {
		'zepto': {
			deps: [],
			exports: '$'
		},
		'common': {
			deps: ['zepto'],
			exports: 'c'
		}
	},
	urlArgs: "rand=20150108" + Math.random(),
	waitSeconds: 60
});