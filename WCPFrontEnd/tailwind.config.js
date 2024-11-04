/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./**/*.{razor,html,cshtml,cs}'],
  theme: {
      extend: {
          animation: {
              'spin-once': 'spin 400ms ease-in-out',
          }
      },
  },
  plugins: [],
}

