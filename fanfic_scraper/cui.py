import os


def pause():
    # Wait for user input.
    _ = input("Press the <ENTER> key to continue...")


def get_entry(text, value):
    # Substitute {0} in text with value.
    return text.format(value)


def get_bool(value):
    ret = ''

    if value == 1:
        ret = "True"
    else:
        ret = "False"

    return ret


def submenu_nav(values, prompt, idx, count, input_value=False, back=False):
    stop = len(values) - 1
    np = 0
    pp = 0
    mm = 0

    while True:
        print(prompt)
        print('')
        for i in range(0, len(values)):
            print(i, values[i])

        if idx < count:
            np = i + 1
            i = i + 1
            print(np, "Next Entry")
        if idx > 0:
            pp = i + 1
            i = i + 1
            print(pp, "Prev Entry")
        if back is True:
            if pp == 0:
                mm = np + 1
                print(mm, "Prev Menu")
            else:
                mm = pp + 1
                print(mm, "Prev Menu")
        print('')

        sel = int(input("Enter Selection: "))

        # Validate input is valid for current menu.
        if (sel >= 0) and (sel <= stop):
            if input_value:
                ret = sel
            else:
                ret = values[sel]
            break
        else:
            # Move 1 page ahead
            if sel == np:
                ret = "Next"
                break
            elif sel == pp:
                ret = "Prev"
                break
            elif sel == mm:
                ret = "Go Back"
                break
            else:
                print("Unknown Option Selected!")
                pause()
                _ = os.system("clear")

    return ret


def submenu(values, prompt, input_value=False):
    inc = 9
    start = 0
    stop = inc
    np = 0
    pp = 0

    while True:
        print(prompt)
        print('')
        for i in range(start, len(values)):
            print(i, values[i])
            j = i
            # limit number of entries to stop var.
            if i >= stop:
                # Stop offering next if the last entry is reached.
                if i < (len(values) - 1):
                    np = stop + 1
                    i = i + 1
                    print(np, "Next Page")
                    break
        # if stop > inc, allow prev page.
        if stop > inc:
            pp = i + 1
            i = i + 1
            print(pp, "Prev Page")
        print('')
        # parse input into number, blank is invalid.
        try:
            sel = int(input("Enter Selection: "))
        except:
            sel = j + 1
        # Validate input is valid for current menu.
        if (sel >= start) and (sel <= j):
            if input_value:
                ret = sel
            else:
                ret = values[sel]
            break
        else:
            # Move 1 page ahead
            if sel == np:
                start = start + 10
                stop = start + 9
                _ = os.system("clear")
            # Move 1 page back
            elif sel == pp:
                start = start - 10
                stop = start + 9
                _ = os.system("clear")
            elif sel < (len(values) - 10):
                start = sel
                stop = start + 9
                _ = os.system("clear")
            else:
                print("Unknown Option Selected!")
                pause()
                _ = os.system("clear")

    return ret


def menu_yesno(title='', clear=True):
    """ Prompt for yes no """

    while True:

        if clear:
            _ = os.system("clear")

        menu = ['Yes', 'No']
        ret = submenu(menu, title)

        if ret == 'Yes':
            return ret
        elif ret == 'No':
            return ret
        else:
            print("Invalid Option!")
            pause()


def check_binary(file_name):

    ret = subprocess.run(["which", file_name],
                         stdout=subprocess.PIPE,
                         stderr=subprocess.PIPE)
    check = ret.stdout.decode('utf-8').splitlines()[0]

    if filename in check:
        ret = 0
    else:
        ret = 1

    return ret