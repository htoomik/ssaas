# Must
- feat(worker): check that url is valid / handle invalid urls
- refactor(config): remove duplication of appsettings.json

# Should
- refactor: store image path in db
- chore: have a clean "starter" db in git; "real" db outside git
- feat(UI): get input from file instead of command line
- tests: ScreenshotTakerTest should validate that the file is a valid image
- feat(UI): provide path to screenshot when request is done
- feat(UI): report failure reason for failed requests
- refactor: argument parsing
- refactor(worker): move logic out of Program, make testable

# Could
- feat(UI): cancel a queued request