from setuptools import setup

setup(name='fanfic-scraper',
      version='0.7.0',
      description='Scrapes fanfics, maintains download db, organized folders for reading.',
      url='https://github.com/KebinuChiousu/fanfic-scraper',
      download_url='https://github.com/KebinuChious/fanfic-scraper/tarball/0.7.0',
      author='Kevin Meredith',
      author_email='kevin@meredithkm.info',
      license='MIT',
      classifiers=[
          'Development Status :: 3 - Alpha',
          'License :: OSI Approved :: MIT License',
          'Programming Language :: Python :: 3.5',
          'Topic :: Games/Entertainment',
      ],
      keywords='fanfic scraper',
      packages=['fanfic_scraper'],
      install_requires=[
          'beautifulsoup4==4.6.0',
          'certifi==2017.7.27.1',
          'chardet==3.0.4',
          'futures==3.1.1',
          'idna==2.6', 
          'requests==2.18.4',
          'urllib3==1.22',
          'html2text==2018.1.9',
          'pony==0.7.3',
          'tldextract==2.2.0'
      ],
      entry_points={
          'console_scripts': [
          'fanfic-scraper=fanfic_scraper.fanfic_scraper:main',
          'ff=fanfic_scraper.ff:main',
          'ffdl=fanfic_scraper.ffdl:main',
          'ff-fif=fanfic_scraper.ff_fif:main',
          'ff-h2t=fanfic_scraper.ff_h2t:main'
        ],
      },
      include_package_data=True,
      zip_safe=False)
