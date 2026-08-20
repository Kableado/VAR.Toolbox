#!/bin/bash
set -e

# ANSI color codes
RED='\033[0;31m'
GREEN='\033[0;32m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

SOURCE_FOLDER=$(cd "$(dirname "$0")" && pwd)
PUBLISH_DIR="$SOURCE_FOLDER/publish"
BINARY_NAME="VAR.Toolbox"
INSTALL_BIN_DIR="$HOME/.bin"
INSTALL_APPS_DIR="$HOME/.local/share/applications"
INSTALL_ICONS_DIR="$HOME/.local/share/icons"
APP_NAME="VAR Toolbox"
ICON_NAME="var-toolbox"

echo -e "${CYAN}Installing $APP_NAME...${NC}"

# Check if published binary exists
if [ ! -f "$PUBLISH_DIR/$BINARY_NAME" ]; then
    echo -e "${RED}Error: Published binary not found at $PUBLISH_DIR/$BINARY_NAME${NC}"
    echo "Please run ./publish.sh first."
    exit 1
fi

# Create directories if they don't exist
mkdir -p "$INSTALL_BIN_DIR"
mkdir -p "$INSTALL_APPS_DIR"
mkdir -p "$INSTALL_ICONS_DIR"

# Install binary
echo "Installing binary to $INSTALL_BIN_DIR..."
cp "$PUBLISH_DIR/$BINARY_NAME" "$INSTALL_BIN_DIR/"
chmod +x "$INSTALL_BIN_DIR/$BINARY_NAME"

# Install icon
echo "Installing icon..."
# Prefer SVG if available for better scaling
if [ -f "$SOURCE_FOLDER/VAR.Toolbox/Images/toolbox.svg" ]; then
    cp "$SOURCE_FOLDER/VAR.Toolbox/Images/toolbox.svg" "$INSTALL_ICONS_DIR/$ICON_NAME.svg"
    ICON_PATH="$ICON_NAME" # Icon name without extension is standard for desktop files
elif [ -f "$SOURCE_FOLDER/VAR.Toolbox/Images/Toolbox.png" ]; then
    cp "$SOURCE_FOLDER/VAR.Toolbox/Images/Toolbox.png" "$INSTALL_ICONS_DIR/$ICON_NAME.png"
    ICON_PATH="$ICON_NAME"
else
    # Fallback if no images found
    ICON_PATH="system-run"
fi

# Create .desktop file
DESKTOP_FILE="$INSTALL_APPS_DIR/var-toolbox.desktop"
echo "Creating desktop entry at $DESKTOP_FILE..."

cat <<EOF > "$DESKTOP_FILE"
[Desktop Entry]
Version=1.0
Type=Application
Name=$APP_NAME
Comment=VAR Toolbox Application
Exec=$INSTALL_BIN_DIR/$BINARY_NAME
Icon=$ICON_PATH
Terminal=false
Categories=Utility;Development;
EOF

chmod +x "$DESKTOP_FILE"

echo ""
echo -e "${GREEN}Installation complete!${NC}"
echo -e "You can now find ${CYAN}$APP_NAME${NC} in your application menu."
echo -e "Note: Ensure ${CYAN}$INSTALL_BIN_DIR${NC} is in your PATH to run it from terminal."
