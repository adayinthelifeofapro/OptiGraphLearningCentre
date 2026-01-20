/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  content: [
    './Layout/**/*.razor',
    './Components/**/*.razor',
    './Pages/**/*.razor',
    './*.razor',
    './wwwroot/index.html'
  ],
  theme: {
    extend: {
      colors: {
        'opti-blue': '#0037FF',
        'opti-dark': '#1a1a2e',
        'opti-accent': '#00D4AA',
        'opti-light': '#f8fafc'
      },
      fontFamily: {
        sans: ['Inter', 'system-ui', '-apple-system', 'sans-serif'],
        mono: ['JetBrains Mono', 'Fira Code', 'monospace']
      }
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
    require('@tailwindcss/typography')
  ]
}
