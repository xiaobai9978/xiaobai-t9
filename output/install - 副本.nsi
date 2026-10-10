; weasel installation script
!include FileFunc.nsh
!include LogicLib.nsh
!include MUI2.nsh
!include x64.nsh
!include winVer.nsh
!include nsProcess.nsh
Unicode true
;--------------------------------
; General
!ifndef WEASEL_VERSION
!define WEASEL_VERSION 2026.08.04
!endif
!ifndef WEASEL_BUILD
!define WEASEL_BUILD 0
!endif
!define WEASEL_ROOT $INSTDIR\xiaobait9-${WEASEL_VERSION}
!define REG_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel"
; The name of the installer
Name "小白T9输入法 ${WEASEL_VERSION}"
; The file to write
OutFile "archives\xiaobait9-${WEASEL_VERSION}-installer.exe"
VIProductVersion "${WEASEL_VERSION}.${WEASEL_BUILD}"
VIAddVersionKey /LANG=2052 "ProductName" "小白T9输入法"
VIAddVersionKey /LANG=2052 "Comments" "Powered by RIME | 中州韻輸入法引擎"
VIAddVersionKey /LANG=2052 "CompanyName" "xiaobai.pro"
VIAddVersionKey /LANG=2052 "LegalCopyright" "Copyleft RIME Developers"
VIAddVersionKey /LANG=2052 "FileDescription" "小白T9输入法"
VIAddVersionKey /LANG=2052 "FileVersion" "${WEASEL_VERSION}"
!define MUI_ICON ..\resource\weasel.ico
SetCompressor lzma
; Request application privileges for Windows Vista
RequestExecutionLevel admin
;--------------------------------
; Pages
!insertmacro MUI_PAGE_LICENSE "LICENSE.txt"
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH
;--------------------------------
; Languages
!insertmacro MUI_LANGUAGE "TradChinese"
LangString DISPLAYNAME ${LANG_TRADCHINESE} "小白T9输入法"
LangString LNKFORMANUAL ${LANG_TRADCHINESE} "【小白T9输入法】說明書"
LangString LNKFORSETTING ${LANG_TRADCHINESE} "【小白T9输入法】輸入法設定"
LangString LNKFORDICT ${LANG_TRADCHINESE} "【小白T9输入法】用戶詞典管理"
LangString LNKFORSYNC ${LANG_TRADCHINESE} "【小白T9输入法】用戶資料同步"
LangString LNKFORNUMKEYBOARD ${LANG_TRADCHINESE} "【小白T9输入法】鍵盤驅動"
LangString LNKFORCONFIG ${LANG_TRADCHINESE} "【小白T9输入法】细节设置"
LangString LNKFORT9SKIN ${LANG_TRADCHINESE} "【小白T9输入法】皮肤编辑器"
LangString LNKFORT9keyboard ${LANG_TRADCHINESE} "【小白T9输入法】软键盘"
LangString LNKFORDEPLOY ${LANG_TRADCHINESE} "【小白T9输入法】重新部署"
LangString LNKFORSERVER ${LANG_TRADCHINESE} "小白T9输入法算法服務"
LangString LNKFORUSERFOLDER ${LANG_TRADCHINESE} "【小白T9输入法】用戶文件夾"
LangString LNKFORAPPFOLDER ${LANG_TRADCHINESE} "【小白T9输入法】程序文件夾"
LangString LNKFORUPDATER ${LANG_TRADCHINESE} "【小白T9输入法】檢查新版本"
LangString LNKFORSETUP ${LANG_TRADCHINESE} "【小白T9输入法】安裝選項"
LangString LNKFORUNINSTALL ${LANG_TRADCHINESE} "卸載小白T9输入法"
LangString CONFIRMATION ${LANG_TRADCHINESE} "安裝前，請先卸載舊版本的小白T9输入法。$\n$\n按下「確定」移除舊版本，按下「取消」放棄本次安裝。"
LangString SYSTEMVERSIONNOTOK ${LANG_TRADCHINESE} "您的系统不被支持，最低系統要求:Windows 8.1!"
!insertmacro MUI_LANGUAGE "SimpChinese"
LangString DISPLAYNAME ${LANG_SIMPCHINESE} "小白T9输入法"
LangString LNKFORMANUAL ${LANG_SIMPCHINESE} "【小白T9输入法】说明书"
LangString LNKFORSETTING ${LANG_SIMPCHINESE} "【小白T9输入法】输入法设定"
LangString LNKFORDICT ${LANG_SIMPCHINESE} "【小白T9输入法】用户词典管理"
LangString LNKFORSYNC ${LANG_SIMPCHINESE} "【小白T9输入法】用户资料同步"
LangString LNKFORNUMKEYBOARD ${LANG_SIMPCHINESE} "【小白T9输入法】键盘驱动"
LangString LNKFORCONFIG ${LANG_SIMPCHINESE} "【小白T9输入法】细节设置"
LangString LNKFORT9SKIN ${LANG_SIMPCHINESE} "【小白T9输入法】皮肤编辑器"
LangString LNKFORT9keyboard ${LANG_SIMPCHINESE} "【小白T9输入法】软键盘"
LangString LNKFORDEPLOY ${LANG_SIMPCHINESE} "【小白T9输入法】重新部署"
LangString LNKFORSERVER ${LANG_SIMPCHINESE} "小白T9输入法算法服务"
LangString LNKFORUSERFOLDER ${LANG_SIMPCHINESE} "【小白T9输入法】用户文件夹"
LangString LNKFORAPPFOLDER ${LANG_SIMPCHINESE} "【小白T9输入法】程序文件夹"
LangString LNKFORUPDATER ${LANG_SIMPCHINESE} "【小白T9输入法】检查新版本"
LangString LNKFORSETUP ${LANG_SIMPCHINESE} "【小白T9输入法】安装选项"
LangString LNKFORUNINSTALL ${LANG_SIMPCHINESE} "卸载小白T9输入法"
LangString CONFIRMATION ${LANG_SIMPCHINESE} '安装前，请先卸载旧版本的小白T9输入法。$\n$\n点击 "确定" 移除旧版本，或点击 "取消" 放弃本次安装。'
LangString SYSTEMVERSIONNOTOK ${LANG_SIMPCHINESE} "您的系統不被支持，最低系统要求:Windows 8.1!"



!insertmacro MUI_LANGUAGE "English"

LangString DISPLAYNAME ${LANG_ENGLISH} "xiaobaiT9"
LangString LNKFORMANUAL ${LANG_ENGLISH} "[xiaobaiT9] Manual"
LangString LNKFORSETTING ${LANG_ENGLISH} "[xiaobaiT9] Settings"
LangString LNKFORDICT ${LANG_ENGLISH} "[xiaobaiT9] Dictionary Manager"
LangString LNKFORSYNC ${LANG_ENGLISH} "[xiaobaiT9] Sync User Profile"
LangString LNKFORNUMKEYBOARD ${LANG_ENGLISH} "[xiaobaiT9] Keyboard driver"
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
LangString CONFIRMATION ${LANG_ENGLISH} "Before installation, please uninstall the old version of Weasel.$\n$\nPress 'OK' to remove the old version, or 'Cancel' to abort installation."
LangString SYSTEMVERSIONNOTOK ${LANG_ENGLISH} "Your system not supported, minimium system required: Windows 8.1!"

; ==================== t9s2t 快捷方式名称 ====================
LangString LNKFORT9S2T ${LANG_TRADCHINESE} "【小白T9输入法】语音输入工具"
LangString LNKFORT9S2T ${LANG_SIMPCHINESE} "【小白T9输入法】语音输入工具"
LangString LNKFORT9S2T ${LANG_ENGLISH} "[xiaobaiT9] Voice Input Tool"
; ============================================================

;--------------------------------
Function .onInit
  ; if not version >= 8.1, quit and MessageBox(if not silent)
  ${IfNot} ${AtLeastWin8.1}
    IfSilent toquit
    MessageBox MB_OK '$(SYSTEMVERSIONNOTOK)'
  toquit:
    Quit
  ${EndIf}
  ; 强制设置安装目录，根据系统架构和 Windows 版本
  ${If} ${IsNativeARM64}
    StrCpy $INSTDIR "$PROGRAMFILES64\Rime"
  ${ElseIf} ${IsNativeAMD64}
    StrCpy $INSTDIR "$PROGRAMFILES64\Rime"
  ${Else}
    StrCpy $INSTDIR "$PROGRAMFILES\Rime"
  ${EndIf}
  ReadRegStr $R0 HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel" "UninstallString"
  StrCmp $R0 "" done
  StrCpy $0 "Upgrade"
  IfSilent uninst 0
  MessageBox MB_OKCANCEL|MB_ICONINFORMATION "$(CONFIRMATION)" IDOK uninst
  Abort
uninst:
  nsExec::ExecToLog 'taskkill /F /IM numkeyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM helpme.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9keyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9configui.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9skin.exe'
  nsExec::ExecToLog 'taskkill /F /IM update.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9s2t.exe'
  Sleep 500
  ; Backup data directory from previous installation, user files may exist
  ReadRegStr $R1 HKLM "Software\Rime\Weasel" "WeaselRoot"
  StrCmp $R1 "" call_uninstaller
  IfFileExists $R1\data\*.* 0 call_uninstaller
  CreateDirectory $TEMP\weasel-backup
  CopyFiles $R1\data\*.* $TEMP\weasel-backup
call_uninstaller:
  ExecWait '"$R1\WeaselServer.exe" /quit'
  ExecWait '"$R1\WeaselSetup.exe" /u'
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Rime"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel"
  ; don't redirect on 64 bit system for auto run setting
  ${If} ${IsNativeARM64}
    SetRegView 64
  ${ElseIf} ${IsNativeAMD64}
    SetRegView 64
  ${EndIf}
  DeleteRegValue HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "WeaselServer"
  ; recover back to 32bit view
  SetRegView 32
  ; Remove files and uninstaller
  Delete "$R1\data\opencc\*.*"
  Delete "$R1\data\preview\*.*"
  Delete "$R1\data\*.*"
  Delete "$R1\*.*"
  RMDir "$R1\data\opencc"
  RMDir "$R1\data\preview"
  RMDir "$R1\data"
  RMDir "$R1"
  SetShellVarContext all
  Delete "$SMPROGRAMS\$(DISPLAYNAME)\*.*"
  RMDir "$SMPROGRAMS\$(DISPLAYNAME)"
  ; Prompt reboot
  SetRebootFlag true
  Sleep 800
done:
FunctionEnd
; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\Rime\Weasel" "InstallDir"
; The stuff to install
Section "Weasel"
  SectionIn RO
  nsExec::ExecToLog 'taskkill /F /IM numkeyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM helpme.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9keyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9configui.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9skin.exe'
  nsExec::ExecToLog 'taskkill /F /IM update.exe'
  Sleep 500

  ; Write the new installation path into the registry
  WriteRegStr HKLM SOFTWARE\Rime\Weasel "InstallDir" "$INSTDIR"
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
  File "update.exe"
  File "helpme.exe"
  File "Newtonsoft.Json.xml"
  File "Newtonsoft.Json.dll"
  File "numkeyboard.exe"
  File "t9keyboard.exe"
  File "t9configui.exe"
  File "t9skin.exe"
  File "Cyotek.Windows.Forms.ColorPicker.dll"
  File "LICENSE.txt"
  File "README.txt"
  File "7-zip-license.txt"
  File "7z.dll"
  File "7z.exe"
  File "COPYING-curl.txt"
  File "curl.exe"
  File "curl-ca-bundle.crt"
  File "rime-install.bat"
  File "rime-install-config.bat"
  File "start_service.bat"
  File "stop_service.bat"
  File "weasel.dll"
  ${If} ${RunningX64}
    File "weaselx64.dll"
  ${EndIf}
  File "weasel.ime"
  ${If} ${RunningX64}
    File "weaselx64.ime"
  ${EndIf}
  ; Always install 32-bit core components (x64 rime.dll has STATUS_STACK_BUFFER_OVERRUN crash bug)
  File "Win32\WeaselDeployer.exe"
  File "Win32\WeaselServer.exe"
  File "Win32\rime.dll"
  File "Win32\WinSparkle.dll"
  ${If} ${RunningX64}
    ; x64: also install 32-bit components to Win32 subdir (backup)
    SetOutPath $INSTDIR\Win32
    File "Win32\WeaselDeployer.exe"
    File "Win32\WeaselServer.exe"
    File "Win32\rime.dll"
    File "Win32\WinSparkle.dll"
    SetOutPath $INSTDIR
  ${EndIf}
  File "WeaselSetup.exe"

  ; ==================== 新增：复制 t9s2t 文件夹 ====================
  SetOutPath $INSTDIR
  File /r "t9s2t"
  ; ============================================================

  ; shared data files
  SetOutPath $INSTDIR\data
  File "data\*.yaml"
  File /nonfatal "data\*.txt"
  File /nonfatal "data\*.gram"

  ; opencc data files
  SetOutPath $INSTDIR\data\dicts
  File "data\dicts\*.yaml"
; 1. 极其重要：强制将上下文切回当前用户，确保 $APPDATA 指向 AppData\Roaming
  SetShellVarContext current

  ; 2. 显式创建 lua 文件夹
  CreateDirectory "$APPDATA\Rime\lua"

  ; 3. 设置输出路径并写入 t9_preedit.lua
  SetOutPath $APPDATA\Rime\lua
  File "t9_preedit.lua"

  ; 4. 回到外层 Rime 目录，写入其他 lua 文件
  SetOutPath $APPDATA\Rime
  File "rime.lua"
  File "data\weasel.custom.yaml"
  SetOutPath $APPDATA\Rime\opencc
  File "data\opencc\emoji*"
  SetOutPath $INSTDIR\data\opencc
  File "data\opencc\*.json"
  File "data\opencc\*.ocd*"
  ; images
  SetOutPath $INSTDIR\data\preview
  File "data\preview\*.png"
  SetOutPath $INSTDIR
  ; test /T flag for zh_TW locale
  StrCpy $R2 "/i"
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
  WriteRegStr HKLM "${REG_UNINST_KEY}" "DisplayIcon" '"$INSTDIR\WeaselServer.exe"'
  WriteRegStr HKLM "${REG_UNINST_KEY}" "DisplayVersion" "${WEASEL_VERSION}.${WEASEL_BUILD}"
  WriteRegStr HKLM "${REG_UNINST_KEY}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegStr HKLM "${REG_UNINST_KEY}" "Publisher" "小白T9输入法"
  WriteRegStr HKLM "${REG_UNINST_KEY}" "URLInfoAbout" "https://xiaobai.pro/"
  WriteRegStr HKLM "${REG_UNINST_KEY}" "HelpLink" "https://t9.xiaobai.pro/?p=254"
  WriteRegDWORD HKLM "${REG_UNINST_KEY}" "NoModify" 1
  WriteRegDWORD HKLM "${REG_UNINST_KEY}" "NoRepair" 1
  WriteUninstaller "$INSTDIR\uninstall.exe"

  ; 添加开机自启：WeaselServer.exe
  ${If} ${IsNativeARM64}
    SetRegView 64
  ${ElseIf} ${IsNativeAMD64}
    SetRegView 64
  ${Else}
    SetRegView 32
  ${EndIf}
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "WeaselServer" '"$INSTDIR\WeaselServer.exe"'

  ; ===== 新增：自动勾选“以管理员身份运行此程序”兼容性选项 =====
  ; 处理 64/32 位注册表视图
  ${If} ${IsNativeARM64}
    SetRegView 64
  ${ElseIf} ${IsNativeAMD64}
    SetRegView 64
  ${Else}
    SetRegView 32
  ${EndIf}
  ; WeaselServer.exe（位于根目录）
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\Win32\WeaselServer.exe" "RUNASADMIN"
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\WeaselServer.exe" "RUNASADMIN"
  ; WeaselDeployer.exe（位于 Win32 子目录）
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\Win32\WeaselDeployer.exe" "RUNASADMIN"
  WriteRegStr HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\WeaselDeployer.exe" "RUNASADMIN"
  SetRegView lastused  ; 恢复默认视图
  ; ===== 新增结束 =====

  IfSilent deploy_silently
  ExecWait "$INSTDIR\WeaselDeployer.exe /install"
  GoTo deploy_done
  deploy_silently:
  ExecWait "$INSTDIR\WeaselDeployer.exe /deploy"
  deploy_done:
  ; Start WeaselServer (后台运行)
  Exec "$INSTDIR\WeaselServer.exe"
  ; 安装完成后强制检查一次更新
  Sleep 3000
  ExecWait '"$INSTDIR\update.exe"' $0
  WriteRegStr HKCU "Software\Rime\Weasel\Updates" "CheckForUpdates" "0"
  ; 升级时提示重启
  StrCmp $0 "Upgrade" 0 +2
  SetRebootFlag true
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"
  SetShellVarContext all
  CreateDirectory "$SMPROGRAMS\$(DISPLAYNAME)"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORMANUAL).lnk" "$INSTDIR\README.txt"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSETTING).lnk" "$INSTDIR\WeaselDeployer.exe" "" "$SYSDIR\shell32.dll" 21
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORDICT).lnk" "$INSTDIR\WeaselDeployer.exe" "/dict" "$SYSDIR\shell32.dll" 6
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSYNC).lnk" "$INSTDIR\WeaselDeployer.exe" "/sync" "$SYSDIR\shell32.dll" 26
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORDEPLOY).lnk" "$INSTDIR\WeaselDeployer.exe" "/deploy" "$SYSDIR\shell32.dll" 144
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSERVER).lnk" "$INSTDIR\WeaselServer.exe" "" "$INSTDIR\WeaselServer.exe" 0
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORNUMKEYBOARD).lnk" "$INSTDIR\numkeyboard.exe" "" "$INSTDIR\numkeyboard.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORCONFIG).lnk" "$INSTDIR\t9configui.exe" "" "$INSTDIR\t9configui.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORT9SKIN).lnk" "$INSTDIR\t9skin.exe" "" "$INSTDIR\t9skin.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORt9keyboard).lnk" "$INSTDIR\t9keyboard.exe" "" "$INSTDIR\t9keyboard.exe"
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUSERFOLDER).lnk" "$INSTDIR\WeaselServer.exe" "/userdir" "$SYSDIR\shell32.dll" 126
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORAPPFOLDER).lnk" "$INSTDIR\WeaselServer.exe" "/weaseldir" "$SYSDIR\shell32.dll" 19
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUPDATER).lnk" "$INSTDIR\WeaselServer.exe" "/update" "$SYSDIR\shell32.dll" 13
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORSETUP).lnk" "$INSTDIR\WeaselSetup.exe" "" "$SYSDIR\shell32.dll" 162
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORUNINSTALL).lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\$(DISPLAYNAME)\$(LNKFORT9S2T).lnk" "$INSTDIR\t9s2t\t9s2t.exe" "" "$INSTDIR\t9s2t\t9s2t.exe" 0


	; 创建桌面快捷方式
  SetShellVarContext all
  ; 参数说明：快捷方式路径、目标程序、参数、图标资源、图标索引
  CreateShortCut "$DESKTOP\小白T9输入法HelpMe.lnk" "$INSTDIR\helpme.exe" "" "$INSTDIR\helpme.exe" 0
SectionEnd

;--------------------------------
; Uninstaller
Section "Uninstall"
  ; ===== 修改一：先注销输入法服务，解决语言栏残留问题 =====
  IfFileExists "$INSTDIR\WeaselSetup.exe" 0 +2
  ExecWait '"$INSTDIR\WeaselSetup.exe" /u'

  ; 尝试结束相关进程
  nsExec::ExecToLog 'taskkill /F /IM numkeyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM helpme.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9keyboard.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9configui.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9skin.exe'
  nsExec::ExecToLog 'taskkill /F /IM update.exe'
  nsExec::ExecToLog 'taskkill /F /IM WeaselServer.exe'
  nsExec::ExecToLog 'taskkill /F /IM t9s2t.exe'
  Sleep 1000 ; 稍微增加等待时间，确保进程完全释放文件句柄

  ; 卸载时清理“以管理员身份运行”兼容性设置
  ${If} ${IsNativeARM64}
    SetRegView 64
  ${ElseIf} ${IsNativeAMD64}
    SetRegView 64
  ${Else}
    SetRegView 32
  ${EndIf}
  DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\WeaselServer.exe"
  DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\WeaselDeployer.exe"
  DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\Win32\WeaselServer.exe"
  DeleteRegValue HKLM "SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers" "$INSTDIR\Win32\WeaselDeployer.exe"
  SetRegView lastused

  ; 删除注册表
  DeleteRegKey HKLM "SOFTWARE\Rime"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Weasel"
  ${If} ${IsNativeARM64}
    SetRegView 64
  ${ElseIf} ${IsNativeAMD64}
    SetRegView 64
  ${EndIf}
  DeleteRegValue HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "WeaselServer"
  SetRegView 32

  ; ===== 修改二：清理文件逻辑优化，避免硬编码路径 =====
  ; 清除安装目录及其内部所有文件的系统/只读/隐藏属性
  nsExec::ExecToLog 'attrib -S -H -R "$INSTDIR\*.*" /D /S'
  ; 强制递归删除安装目录
  RMDir /r /REBOOTOK "$INSTDIR"

  ; 安全清理父级 Rime 目录（只有当 Rime 文件夹内没有其他文件时才会删除成功）
  StrCpy $0 "$INSTDIR"
  GetFullPathName $1 "$0\.."
  RMDir "$1"

  ; 删除开始菜单快捷方式
  SetShellVarContext all
  Delete "$SMPROGRAMS\$(DISPLAYNAME)\*.*"
  RMDir "$SMPROGRAMS\$(DISPLAYNAME)"

  ; 删除桌面快捷方式
  Delete "$DESKTOP\$(LNKFORSERVER).lnk"
  Delete "$DESKTOP\小白T9输入法HelpMe.lnk"

  ; 设置重启标记
  SetRebootFlag true
SectionEnd
