; weasel installation script
!include FileFunc.nsh
!include LogicLib.nsh
!include MUI2.nsh
!include x64.nsh

Unicode true

;--------------------------------
; General

!ifndef WEASEL_VERSION
!define WEASEL_VERSION 2023.06.14
!endif

!ifndef WEASEL_BUILD
!define WEASEL_BUILD 0
!endif

!define WEASEL_ROOT $INSTDIR\xiaobait9-${WEASEL_VERSION}
!define REG_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel"

; The name of the installer
Name "С��T9���뷨 ${WEASEL_VERSION}"

; The file to write
OutFile "archives\xiaobait9-${WEASEL_VERSION}.${WEASEL_BUILD}-installer.exe"

VIProductVersion "${WEASEL_VERSION}.${WEASEL_BUILD}"
VIAddVersionKey /LANG=2052 "ProductName" "С��T9���뷨"
VIAddVersionKey /LANG=2052 "Comments" "Powered by RIME | �����ݔ�뷨����"
VIAddVersionKey /LANG=2052 "CompanyName" "xiaobai.pro"
VIAddVersionKey /LANG=2052 "LegalCopyright" "Copyleft RIME Developers"
VIAddVersionKey /LANG=2052 "FileDescription" "С��T9���뷨"
VIAddVersionKey /LANG=2052 "FileVersion" "${WEASEL_VERSION}"

!define MUI_ICON ..\resource\weasel.ico
SetCompressor /SOLID lzma

; The default installation directory
InstallDir $PROGRAMFILES\xiaobait9

; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Rime\Weasel" "InstallDir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

;--------------------------------

; Languages

!insertmacro MUI_LANGUAGE "TradChinese"
LangString DISPLAYNAME ${LANG_TRADCHINESE} "С��T9���뷨"
LangString LNKFORMANUAL ${LANG_TRADCHINESE} "��С��T9���뷨���f����"
LangString LNKFORSETTING ${LANG_TRADCHINESE} "��С��T9���뷨��ݔ�뷨�O��"
LangString LNKFORDICT ${LANG_TRADCHINESE} "��С��T9���뷨���Ñ��~�����"
LangString LNKFORSYNC ${LANG_TRADCHINESE} "��С��T9���뷨���Ñ��Y��ͬ��"
LangString LNKFORCONFIG ${LANG_TRADCHINESE} "��С��T9���뷨��ϸ������"
LangString LNKFORT9SKIN ${LANG_TRADCHINESE} "��С��T9���뷨��Ƥ���༭��"
LangString LNKFORT9keyboard ${LANG_TRADCHINESE} "��С��T9���뷨�������"
LangString LNKFORDEPLOY ${LANG_TRADCHINESE} "��С��T9���뷨�����²���"
LangString LNKFORSERVER ${LANG_TRADCHINESE} "С��T9���뷨�㷨����"
LangString LNKFORUSERFOLDER ${LANG_TRADCHINESE} "��С��T9���뷨���Ñ��ļ��A"
LangString LNKFORAPPFOLDER ${LANG_TRADCHINESE} "��С��T9���뷨�������ļ��A"
LangString LNKFORUPDATER ${LANG_TRADCHINESE} "��С��T9���뷨���z���°汾"
LangString LNKFORSETUP ${LANG_TRADCHINESE} "��С��T9���뷨�����b�x�"
LangString LNKFORUNINSTALL ${LANG_TRADCHINESE} "ж�dС��T9���뷨"

!insertmacro MUI_LANGUAGE "SimpChinese"
LangString DISPLAYNAME ${LANG_SIMPCHINESE} "С��T9���뷨"
LangString LNKFORMANUAL ${LANG_SIMPCHINESE} "��С��T9���뷨��˵����"
LangString LNKFORSETTING ${LANG_SIMPCHINESE} "��С��T9���뷨�����뷨�趨"
LangString LNKFORDICT ${LANG_SIMPCHINESE} "��С��T9���뷨���û��ʵ����"
LangString LNKFORSYNC ${LANG_SIMPCHINESE} "��С��T9���뷨���û�����ͬ��"
LangString LNKFORCONFIG ${LANG_SIMPCHINESE} "��С��T9���뷨��ϸ������"
LangString LNKFORT9SKIN ${LANG_SIMPCHINESE} "��С��T9���뷨��Ƥ���༭��"
LangString LNKFORT9keyboard ${LANG_SIMPCHINESE} "��С��T9���뷨�������"
LangString LNKFORDEPLOY ${LANG_SIMPCHINESE} "��С��T9���뷨�����²���"
LangString LNKFORSERVER ${LANG_SIMPCHINESE} "С��T9���뷨�㷨����"
LangString LNKFORUSERFOLDER ${LANG_SIMPCHINESE} "��С��T9���뷨���û��ļ���"
LangString LNKFORAPPFOLDER ${LANG_SIMPCHINESE} "��С��T9���뷨�������ļ���"
LangString LNKFORUPDATER ${LANG_SIMPCHINESE} "��С��T9���뷨������°汾"
LangString LNKFORSETUP ${LANG_SIMPCHINESE} "��С��T9���뷨����װѡ��"
LangString LNKFORUNINSTALL ${LANG_SIMPCHINESE} "ж��С��T9���뷨"

!insertmacro MUI_LANGUAGE "English"
LangString DISPLAYNAME ${LANG_ENGLISH} "xiaobaiT9"
LangString LNKFORMANUAL ${LANG_ENGLISH} "[xiaobaiT9] Manual"
LangString LNKFORSETTING ${LANG_ENGLISH} "[xiaobaiT9] Settings"
LangString LNKFORDICT ${LANG_ENGLISH} "[xiaobaiT9] Dictionary Manager"
LangString LNKFORSYNC ${LANG_ENGLISH} "[xiaobaiT9] Sync User Profile"
LangString LNKFORCONFIG ${LANG_ENGLISH} "[xiaobaiT9] Config"
LangString LNKFORT9SKIN ${LANG_ENGLISH} "[xiaobaiT9] T9 Skin"
LangString LNKFORT9keyboard ${LANG_ENGLISH} "[xiaobaiT9] T9 keyboard"
LangString LNKFORDEPLOY ${LANG_ENGLISH} "[xiaobaiT9] Deploy"
LangString LNKFORSERVER ${LANG_ENGLISH} "xiaobaiT9 Server"
LangString LNKFORUSERFOLDER ${LANG_ENGLISH} "[xiaobaiT9] User Folder"
LangString LNKFORAPPFOLDER ${LANG_ENGLISH} "[xiaobaiT9] App Folder"
LangString LNKFORUPDATER ${LANG_ENGLISH} "[xiaobaiT9] Check for Updates"
LangString LNKFORSETUP ${LANG_ENGLISH} "[xiaobaiT9] Installation Preference"
LangString LNKFORUNINSTALL ${LANG_ENGLISH} "Uninstall xiaobaiT9"
;--------------------------------

Function .onInit
  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\xiaobait9" \
  "UninstallString"
  StrCmp $R0 "" done

  StrCpy $0 "Upgrade"
  IfSilent uninst 0
  MessageBox MB_OKCANCEL|MB_ICONINFORMATION \
  "��װǰ�����bǰ�Ҵ�����ж�ؾɰ汾��С��T9���뷨��$\n$\n���¡�ȷ�����Ƴ��ɰ汾�����¡�ȡ�����������ΰ��b��" \
  IDOK uninst
  Abort

uninst:
  ; Backup data directory from previous installation, user files may exist
  ReadRegStr $R1 HKLM SOFTWARE\Rime\xiaobait9 "WeaselRoot"
  StrCmp $R1 "" call_uninstaller
  IfFileExists $R1\data\*.* 0 call_uninstaller
  CreateDirectory $TEMP\weasel-backup
  CopyFiles $R1\data\*.* $TEMP\weasel-backup

call_uninstaller:
  ExecWait '$R0 /S'
  Sleep 800

done:
FunctionEnd

; The stuff to install
Section "xiaobait9"

  SectionIn RO

  ; Write the new installation path into the registry
  WriteRegStr HKLM SOFTWARE\Rime\xiaobait9 "InstallDir" "$INSTDIR"

  ; Reset INSTDIR for the new version
  StrCpy $INSTDIR "${WEASEL_ROOT}"

  IfFileExists "$INSTDIR\WeaselServer.exe" 0 +2
  ExecWait '"$INSTDIR\WeaselServer.exe" /quit'

  SetOverwrite try
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  IfFileExists $TEMP\weasel-backup\*.* 0 program_files
  CreateDirectory $INSTDIR\data
  CopyFiles $TEMP\weasel-backup\*.* $INSTDIR\data
  RMDir /r $TEMP\weasel-backup

program_files:
  File "t9keyboard.exe"
  File "t9configui.exe"
  File "t9skin.exe"
  File "LICENSE.txt"
  File "README.txt"
  File "README.txt"
  File "7-zip-license.txt"
  File "7z.dll"
  File "7z.exe"
  File "COPYING-curl.txt"
  File "curl.exe"
  File "curl-ca-bundle.crt"
  File "rime-install.bat"
  File "rime-install-config.bat"
  File "weasel.dll"
  ${If} ${RunningX64}
    File "weaselx64.dll"
  ${EndIf}
  File "weaselt.dll"
  ${If} ${RunningX64}
    File "weaseltx64.dll"
  ${EndIf}
  File "weasel.ime"
  ${If} ${RunningX64}
    File "weaselx64.ime"
  ${EndIf}
  File "weaselt.ime"
  ${If} ${RunningX64}
    File "weaseltx64.ime"
  ${EndIf}
  File "WeaselDeployer.exe"
  File "WeaselServer.exe"
  File "WeaselSetup.exe"
  File "rime.dll"
  File "WinSparkle.dll"
  ; shared data files
  SetOutPath $INSTDIR\data
  File "data\*.yaml"
  File /nonfatal "data\*.txt"
  File /nonfatal "data\*.gram"

  SetOutPath $INSTDIR\data\dicts
  File "data\dicts\*.yaml"

  SetOutPath $APPDATA\Rime
  File "*.lua"
  File "data\weasel.custom.yaml"


  SetOutPath $APPDATA\Rime\opencc
;  File "data\opencc\biaodianfuhao.txt"
  File "data\opencc\emoji*"


  ; opencc data files
  SetOutPath $INSTDIR\data\opencc
  File "data\opencc\*.json"
  File "data\opencc\*.ocd*"
  ; images
  SetOutPath $INSTDIR\data\preview
  File "data\preview\*.png"

  SetOutPath $INSTDIR

  ; test /T flag for zh_TW locale
  StrCpy $R2  "/i"
  ${GetParameters} $R0
  ClearErrors
  ${GetOptions} $R0 "/S" $R1
  IfErrors +2 0
  StrCpy $R2 "/s"
  ${GetOptions} $R0 "/T" $R1
  IfErrors +2 0
  StrCpy $R2 "/t"

  ExecWait '"$INSTDIR\WeaselSetup.exe" $R2'

  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "${REG_UNINST_KEY}" "DisplayName" "$(DISPLAYNAME)"
  WriteRegStr HKLM "${REG_UNINST_KEY}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegStr HKLM "${REG_UNINST_KEY}"  "DisplayVersion" "${WEASEL_VERSION}.${WEASEL_BUILD}"
  WriteRegStr HKLM "${REG_UNINST_KEY}"  "Publisher" "С��T9���뷨"
  WriteRegStr HKLM "${REG_UNINST_KEY}"  "URLInfoAbout" "https://xiaobai.pro/"
  WriteRegStr HKLM "${REG_UNINST_KEY}"  "HelpLink" "https://note.youdao.com/s/GFwBIBK2"
  WriteRegDWORD HKLM "${REG_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${REG_UNINST_KEY}" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"

  ; run as user...
  ExecWait "$INSTDIR\WeaselDeployer.exe /install"

  ; Write autorun key
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "WeaselServer" "$INSTDIR\WeaselServer.exe"
  ; Start WeaselServer
  Exec "$INSTDIR\WeaselServer.exe"

  ; Prompt reboot
  StrCmp $0 "Upgrade" 0 +2
  SetRebootFlag true

SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"
  SetShellVarContext all
  CreateDirectory "$SMPROGRAMS\$(DISPLAYNAME)"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORMANUAL).lnk" "https://note.youdao.com/s/GFwBIBK2"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSETTING).lnk" "$INSTDIR\WeaselDeployer.exe" "" "$SYSDIR\shell32.dll" 21
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORDICT).lnk" "$INSTDIR\WeaselDeployer.exe" "/dict" "$SYSDIR\shell32.dll" 6
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSYNC).lnk" "$INSTDIR\WeaselDeployer.exe" "/sync" "$SYSDIR\shell32.dll" 26
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORDEPLOY).lnk" "$INSTDIR\WeaselDeployer.exe" "/deploy" "$SYSDIR\shell32.dll" 144
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSERVER).lnk" "$INSTDIR\WeaselServer.exe" "" "$INSTDIR\WeaselServer.exe" 0
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORCONFIG).lnk" "$INSTDIR\t9configui.exe" "" "$INSTDIR\t9configui.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORT9SKIN).lnk" "$INSTDIR\t9skin.exe" "" "$INSTDIR\t9skin.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORt9keyboard).lnk" "$INSTDIR\t9keyboard.exe" "" "$INSTDIR\t9keyboard.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUSERFOLDER).lnk" "$INSTDIR\WeaselServer.exe" "/userdir" "$SYSDIR\shell32.dll" 126
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORAPPFOLDER).lnk" "$INSTDIR\WeaselServer.exe" "/weaseldir" "$SYSDIR\shell32.dll" 19
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUPDATER).lnk" "$INSTDIR\WeaselServer.exe" "/update" "$SYSDIR\shell32.dll" 13
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSETUP).lnk" "$INSTDIR\WeaselSetup.exe" "" "$SYSDIR\shell32.dll" 162
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUNINSTALL).lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0

SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"

  ExecWait '"$INSTDIR\WeaselServer.exe" /quit'

  ExecWait '"$INSTDIR\WeaselSetup.exe" /u'

  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel"
  DeleteRegValue HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "WeaselServer"
  DeleteRegKey HKLM SOFTWARE\Rime

  ; Remove files and uninstaller
  SetOutPath $TEMP
  Delete /REBOOTOK "$INSTDIR\data\opencc\*.*"
  Delete /REBOOTOK "$INSTDIR\data\preview\*.*"
  Delete /REBOOTOK "$INSTDIR\data\*.*"
  Delete /REBOOTOK "$INSTDIR\*.*"
  RMDir /REBOOTOK "$INSTDIR\data\opencc"
  RMDir /REBOOTOK "$INSTDIR\data\preview"
  RMDir /REBOOTOK "$INSTDIR\data"
  RMDir /REBOOTOK "$INSTDIR"
  SetShellVarContext all
  Delete /REBOOTOK "$SMPROGRAMS\$(DISPLAYNAME)\*.*"
  RMDir /REBOOTOK "$SMPROGRAMS\$(DISPLAYNAME)"

  ; Prompt reboot
  SetRebootFlag true

SectionEnd
