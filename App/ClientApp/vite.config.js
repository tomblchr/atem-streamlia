import { defineConfig } from "vite";

export default defineConfig((config) => {

    // concatenate current date and commit hash
    // githash currently failing https://github.com/Azure/static-web-apps/issues/1271
    const githash = "unknown"; //require('child_process').execSync('git rev-parse --short HEAD').toString();
    const appversion = JSON.stringify(new Date().toISOString().split('T')[0] + '-' + githash);
    
    console.log(`Build version: ${appversion}`);
    
    return {
        define: {
            __APP_COMMIT_HASH__: appversion
        }
    }
});