#----------------------------------------------------------------
# Generated CMake target import file for configuration "Debug".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "Boost::nowide" for configuration "Debug"
set_property(TARGET Boost::nowide APPEND PROPERTY IMPORTED_CONFIGURATIONS DEBUG)
set_target_properties(Boost::nowide PROPERTIES
  IMPORTED_LINK_INTERFACE_LANGUAGES_DEBUG "CXX"
  IMPORTED_LOCATION_DEBUG "${_IMPORT_PREFIX}/lib/boost_nowide.lib"
  )

list(APPEND _IMPORT_CHECK_TARGETS Boost::nowide )
list(APPEND _IMPORT_CHECK_FILES_FOR_Boost::nowide "${_IMPORT_PREFIX}/lib/boost_nowide.lib" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
