"""Define current fanfic class based on url."""

from fanfic_scraper.extractors.fanfictionnet import FanfictionNetFanfic
from fanfic_scraper.extractors.hpfanficarchive import HPFanficArchive

def check_url(url):
    ret = False

    if 'fanfiction.net' in url:
        return True
    if 'fanfictionproxy.net' in url:
        return True
    if 'hpfanficarchive.com' in url:
        return True

    return ret


def fanfic(fanfic_url, args):
    """Send the approriate class."""
    if 'fanfiction.net' in fanfic_url:
        return FanfictionNetFanfic(fanfic_url, args)
    if 'fanfictionproxy.net' in fanfic_url:
        return FanfictionNetFanfic(fanfic_url, args)
    if 'hpfanficarchive.com' in fanfic_url:
        return HPFanficArchive(fanfic_url, args)
