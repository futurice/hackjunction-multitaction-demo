exists($$(CORNERSTONE_SDK_ROOT)/cornerstone.pri):include($$(CORNERSTONE_SDK_ROOT)/cornerstone.pri)
else {
  exists($$(CORNERSTONE_ROOT_2_0_8)/cornerstone.pri):include($$(CORNERSTONE_ROOT_2_0_8)/cornerstone.pri)
}

TEMPLATE = app
LIBS += $$MULTI_USUAL_LIBS $$MULTI_THREADED_RENDERING

QT += network xml

SOURCES += src/Main.cpp \
    src/MainWidget.cpp \
    src/PopupWidget.cpp \
    src/TweetPuller.cpp

HEADERS += \
    src/MainWidget.hpp \
    src/PopupWidget.hpp \
    src/TweetPuller.hpp

CONFIG += console

OTHER_FILES += style.css
