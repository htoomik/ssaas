# Must
- feat(worker): check that url is valid / handle invalid urls
- feat(worker): mark failed requests as Failed, store reason

# Should
- refactor: store image path in db
- chore: have a clean "starter" db in git; "real" db outside git
- feat(UI): get input from file instead of command line
- tests: ScreenshotTakerTest should validate that the file is a valid image
- feat(UI): provide path to screenshot when request is done
- feat(UI): report failure reason for failed requests

# Could
- feat(worker): log actions to console
- feat(UI): cancel a queued request