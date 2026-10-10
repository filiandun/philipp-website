import { getHighlighter } from 'https://esm.run/shiki@1.22.0';

let highlighter = null;
let customThemeName = '';

export async function init(theme, languages) {
    if (highlighter) return;

    const themeResponse = await fetch(theme);
    const darkVsTheme = await themeResponse.json();

    customThemeName = darkVsTheme.name;

    highlighter = await getHighlighter({
        themes: [darkVsTheme],
        langs: languages
    });
}

export function highlight(code, lang) {
    if (!highlighter) return "";
    if (!highlighter.getLoadedLanguages().includes(lang)) {
        lang = 'csharp';
    }
    return highlighter.codeToHtml(code, { lang: lang, theme: customThemeName });
}