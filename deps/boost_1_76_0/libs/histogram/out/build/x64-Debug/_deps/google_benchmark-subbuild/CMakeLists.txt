# Distributed under the OSI-approved BSD 3-Clause License.  See accompanying
# file Copyright.txt or https://cmake.org/licensing for details.

cmake_minimum_required(VERSION 3.20.21032501-MSVC_2)

# We name the project and the target for the ExternalProject_Add() call
# to something that will highlight to the user what we are working on if
# something goes wrong and an error message is produced.

project(google_benchmark-populate NONE)

# Pass through things we've already detected in the main project to avoid
# paying the cost of redetecting them again in ExternalProject_Add()
set(GIT_EXECUTABLE [==[C:/Program Files/Git/bin/git.exe]==])
set(GIT_VERSION_STRING [==[2.38.0.windows.1]==])
set_property(GLOBAL PROPERTY _CMAKE_FindGit_GIT_EXECUTABLE_VERSION
  [==[C:/Program Files/Git/bin/git.exe;2.38.0.windows.1]==]
)


include(ExternalProject)
ExternalProject_Add(google_benchmark-populate
                     "UPDATE_DISCONNECTED" "False" "GIT_REPOSITORY" "https://github.com/google/benchmark" "GIT_TAG" "master" "GIT_SHALLOW" "1"
                    SOURCE_DIR          "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/google_benchmark-src"
                    BINARY_DIR          "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/google_benchmark-build"
                    CONFIGURE_COMMAND   ""
                    BUILD_COMMAND       ""
                    INSTALL_COMMAND     ""
                    TEST_COMMAND        ""
                    USES_TERMINAL_DOWNLOAD  YES
                    USES_TERMINAL_UPDATE    YES
)


