#!/bin/bash
set -e

# ANSI color codes
RED='\033[0;31m'
GREEN='\033[0;32m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

SOURCE_FOLDER=$(cd "$(dirname "$0")" && pwd)
PROJECT_FILE="$SOURCE_FOLDER/VAR.Toolbox/VAR.Toolbox.csproj"
PUBLISH_DIR="$SOURCE_FOLDER/publish"

echo "Publishing VAR.Toolbox as self-contained..."
echo "  Project: $PROJECT_FILE"
echo "  Publishing to: $PUBLISH_DIR"
echo ""

# Remove existing publish directory
if [ -d "$PUBLISH_DIR" ]; then
    echo "Removing existing publish directory..."
    rm -rf "$PUBLISH_DIR"
fi

# Publish self-contained for Linux x64
dotnet publish "$PROJECT_FILE" \
    -c Release \
    -r linux-x64 \
    -p:SelfContained=true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=false \
    -p:IncludeNativeLibrariesForSelfExtract=true \
    -p:PublishReadyToRun=false \
    -o "$PUBLISH_DIR"

if [ $? -ne 0 ]; then
    echo -e "${RED}Publish failed!${NC}"
    exit 1
fi

echo ""
echo -e "${GREEN}Published successfully to: $PUBLISH_DIR${NC}"

# Find executable (file with the same name as the project/assembly, usually)
# For a single-file publish on Linux, it's usually just the binary name.
EXECUTABLE_NAME="VAR.Toolbox"
if [ -f "$PUBLISH_DIR/$EXECUTABLE_NAME" ]; then
    echo -e "  Executable: ${CYAN}$PUBLISH_DIR/$EXECUTABLE_NAME${NC}"
fi
