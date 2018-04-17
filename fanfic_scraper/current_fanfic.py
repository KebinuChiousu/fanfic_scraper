"""Define current fanfic class based on url."""

from fanfic_scraper.extractors.fanfictionnet import FanfictionNetFanfic

def fanfic(fanfic_url, args, verify_https):
    """Send the approriate class."""
    if 'fanfiction.net' in fanfic_url:
        return FanfictionNetFanfic(fanfic_url, args, verify_https)
