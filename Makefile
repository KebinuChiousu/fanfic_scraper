PROJECT_NAME := $(shell python3 setup.py --name)
PROJECT_VERSION := $(shell python3 setup.py --version)

SHELL := /bin/bash
BOLD := \033[1m
DIM := \033[2m
RESET := \033[0m

.PHONY: all
all: uninstall install clean

.PHONY: develop
develop:
	@echo -e "$(BOLD)installing in develop mode $(PROJECT_NAME) $(PROJECT_VERSION)$(RESET)"
	@echo -e -n "$(DIM)"
	@pip install -e .
	@echo -e -n "$(RESET)"

.PHONY: install
install:
	@echo -e "$(BOLD)installing $(PROJECT_NAME) $(PROJECT_VERSION)$(RESET)"
	@echo -e -n "$(DIM)"
	@pip3 install .
	@echo -e -n "$(RESET)"

.PHONY: uninstall
uninstall:
	@echo -e "$(BOLD)uninstalling '$(PROJECT_NAME)'$(RESET)"
	-@pip3 uninstall -y $(PROJECT_NAME) 2> /dev/null

.PHONY: dist
dist:
	@echo -e "$(BOLD)packaging $(PROJECT_NAME) $(PROJECT_VERSION)$(RESET)"
	@echo -e -n "$(DIM)"
	@python3 setup.py sdist --dist-dir=dist
	@echo -e -n "$(RESET)"

.PHONY: release
release:
	twine upload dist/*

.PHONY: clean
clean:
	@echo -e "$(BOLD)cleaning $(PROJECT_NAME) $(PROJECT_VERSION) repository$(RESET)"
	@rm -rf build dist $(PROJECT_NAME).egg-info
