import { defineConfig } from "vite";

export default defineConfig((config) => {

    // concatenate current date and commit hash
    const appversion = JSON.stringify(new Date().toISOString().split('T')[0] + '-' + require('child_process').execSync('git rev-parse --short HEAD').toString());
    
    console.log(`Build version: ${appversion}`);
    
    return {
        define: {
            __APP_COMMIT_HASH__: appversion
        }
    }
});