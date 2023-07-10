#include "stdafx.h"
#include "WeaselServerApp.h"

WeaselServerApp::WeaselServerApp()
	: m_handler(std::make_unique<RimeWithWeaselHandler>(&m_ui))
	, tray_icon(m_ui)
{
	//m_handler.reset(new RimeWithWeaselHandler(&m_ui));
	m_server.SetRequestHandler(m_handler.get());
	SetupMenuHandlers();
}

WeaselServerApp::~WeaselServerApp()
{
}

int WeaselServerApp::Run()
{
	if (!m_server.Start())
		return -1;

	//win_sparkle_set_appcast_url("http://localhost:8000/weasel/update/appcast.xml");
	win_sparkle_set_registry_path("Software\\Rime\\Weasel\\Updates");
	win_sparkle_init();
	m_ui.Create(m_server.GetHWnd());

	tray_icon.Create(m_server.GetHWnd());
	tray_icon.Refresh();

	m_handler->Initialize();
	m_handler->OnUpdateUI([this]() {
		tray_icon.Refresh();
	});

	int ret = m_server.Run();

	m_handler->Finalize();
	m_ui.Destroy();
	tray_icon.RemoveIcon();
	win_sparkle_cleanup();

	return ret;
}

void WeaselServerApp::SetupMenuHandlers()
{
	std::wstring dir(install_dir());
	m_server.AddMenuHandler(ID_WEASELTRAY_QUIT, [this] { return m_server.Stop() == 0; });
	m_server.AddMenuHandler(ID_WEASELTRAY_DEPLOY, std::bind(execute, dir + L"\\WeaselDeployer.exe", std::wstring(L"/deploy")));
	m_server.AddMenuHandler(ID_WEASELTRAY_SETTINGS, std::bind(execute, dir + L"\\WeaselDeployer.exe", std::wstring()));
	m_server.AddMenuHandler(ID_WEASELTRAY_T9KEYBOARD, std::bind(execute, dir + L"\\t9keyboard.exe", std::wstring()));
	m_server.AddMenuHandler(ID_WEASELTRAY_SETTING, std::bind(execute, dir + L"\\t9configui.exe", std::wstring()));
	m_server.AddMenuHandler(ID_WEASELTRAY_T9SKIN, std::bind(execute, dir + L"\\t9skin.exe", std::wstring()));
	m_server.AddMenuHandler(ID_WEASELTRAY_DICT_MANAGEMENT, std::bind(execute, dir + L"\\WeaselDeployer.exe", std::wstring(L"/dict")));
	m_server.AddMenuHandler(ID_WEASELTRAY_SYNC, std::bind(execute, dir + L"\\WeaselDeployer.exe", std::wstring(L"/sync")));
	m_server.AddMenuHandler(ID_WEASELTRAY_WIKI, std::bind(open, L"https://note.youdao.com/s/GFwBIBK2"));
	m_server.AddMenuHandler(ID_WEASELTRAY_HOMEPAGE, std::bind(open, L"https://xiaobai.pro"));
	m_server.AddMenuHandler(ID_WEASELTRAY_FORUM, std::bind(open, L"http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=iZRDO_bhNFUbE-mOz9txGgSqLk4txNAi&authKey=Z8iFEhYbqTGThLF0xOmqeMCD73gkQUzqCKy7BK%2BBA6Wv1uPL7XgxRoIym3SJqx1x&noverify=0&group_code=387170746"));
	//m_server.AddMenuHandler(ID_WEASELTRAY_CHECKUPDATE, check_update);
	m_server.AddMenuHandler(ID_WEASELTRAY_CHECKUPDATE, std::bind(open, L"https://t9.xiaobai.pro/"));
	m_server.AddMenuHandler(ID_WEASELTRAY_INSTALLDIR, std::bind(explore, dir));
	m_server.AddMenuHandler(ID_WEASELTRAY_USERCONFIG, std::bind(explore, WeaselUserDataPath()));
}
