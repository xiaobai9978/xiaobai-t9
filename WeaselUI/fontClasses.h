#pragma once
#include <WeaselCommon.h>
#include <WeaselUI.h>
#include <vector>
#include <regex>
#include <iterator>
#include <d2d1.h>
#include <dwrite_2.h>

using namespace weasel;


namespace weasel
{
	template <class T> void SafeRelease(T** ppT)
	{
		if (*ppT)
		{
			(*ppT)->Release();
			*ppT = NULL;
		}
	}
	class DirectWriteResources
	{
	public:
		DirectWriteResources(weasel::UIStyle& style, UINT dpi);
		~DirectWriteResources();

		HRESULT InitResources(std::wstring label_font_face, int label_font_point,
			std::wstring font_face, int font_point,
			std::wstring comment_font_face, int comment_font_point, bool vertical_text = false);
		HRESULT InitResources(UIStyle& style, UINT dpi);
		void SetDpi(UINT dpi);
		float dpiScaleX_, dpiScaleY_;
		ID2D1Factory* pD2d1Factory;
		IDWriteFactory2* pDWFactory;
		ID2D1DCRenderTarget* pRenderTarget;
		IDWriteTextFormat1* pPreeditTextFormat;
		IDWriteTextFormat1* pTextFormat;
		IDWriteTextFormat1* pLabelTextFormat;
		IDWriteTextFormat1* pCommentTextFormat;
		IDWriteTextLayout2* pTextLayout;
	private:
		UIStyle& _style;
		void _ParseFontFace(const std::wstring fontFaceStr, DWRITE_FONT_WEIGHT& fontWeight, DWRITE_FONT_STYLE& fontStyle);
		void _SetFontFallback(IDWriteTextFormat1* pTextFormat, std::vector<std::wstring> fontVector);
	};
};
