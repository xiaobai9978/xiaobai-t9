
if(NOT "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitinfo.txt" IS_NEWER_THAN "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitclone-lastrun.txt")
  message(STATUS "Avoiding repeated git clone, stamp file is up to date: 'C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitclone-lastrun.txt'")
  return()
endif()

execute_process(
  COMMAND ${CMAKE_COMMAND} -E rm -rf "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-src"
  RESULT_VARIABLE error_code
  )
if(error_code)
  message(FATAL_ERROR "Failed to remove directory: 'C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-src'")
endif()

# try the clone 3 times in case there is an odd git clone issue
set(error_code 1)
set(number_of_tries 0)
while(error_code AND number_of_tries LESS 3)
  execute_process(
    COMMAND "C:/Program Files/Git/bin/git.exe"  clone --no-checkout --depth 1 --no-single-branch --config "advice.detachedHead=false" "https://github.com/boostorg/variant2" "boostorg_variant2-src"
    WORKING_DIRECTORY "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps"
    RESULT_VARIABLE error_code
    )
  math(EXPR number_of_tries "${number_of_tries} + 1")
endwhile()
if(number_of_tries GREATER 1)
  message(STATUS "Had to git clone more than once:
          ${number_of_tries} times.")
endif()
if(error_code)
  message(FATAL_ERROR "Failed to clone repository: 'https://github.com/boostorg/variant2'")
endif()

execute_process(
  COMMAND "C:/Program Files/Git/bin/git.exe"  checkout develop --
  WORKING_DIRECTORY "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-src"
  RESULT_VARIABLE error_code
  )
if(error_code)
  message(FATAL_ERROR "Failed to checkout tag: 'develop'")
endif()

set(init_submodules TRUE)
if(init_submodules)
  execute_process(
    COMMAND "C:/Program Files/Git/bin/git.exe"  submodule update --recursive --init 
    WORKING_DIRECTORY "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-src"
    RESULT_VARIABLE error_code
    )
endif()
if(error_code)
  message(FATAL_ERROR "Failed to update submodules in: 'C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-src'")
endif()

# Complete success, update the script-last-run stamp file:
#
execute_process(
  COMMAND ${CMAKE_COMMAND} -E copy
    "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitinfo.txt"
    "C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitclone-lastrun.txt"
  RESULT_VARIABLE error_code
  )
if(error_code)
  message(FATAL_ERROR "Failed to copy script-last-run stamp file: 'C:/weasel/deps/boost_1_76_0/libs/histogram/out/build/x64-Debug/_deps/boostorg_variant2-subbuild/boostorg_variant2-populate-prefix/src/boostorg_variant2-populate-stamp/boostorg_variant2-populate-gitclone-lastrun.txt'")
endif()

